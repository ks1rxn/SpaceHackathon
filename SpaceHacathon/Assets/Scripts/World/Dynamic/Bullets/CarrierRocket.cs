using System.Collections;
using UnityEngine;

public class CarrierRocket : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private float m_detonatorActivateTime;

	[SerializeField]
	private CollisionDetector m_collisionDetector;
	[SerializeField]
	private ParticleSystem m_trail;

	private float m_power;
	private bool m_inactive;

	private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

	private float m_maxSpeed = 0;
	private float m_maxRotationSpeed = 0;

	private Vector3 m_target;

	public void Initiate() {
		m_collisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
		m_collisionDetector.RegisterListener(CollisionTags.RamShip, OnTargetHit);
		m_collisionDetector.RegisterListener(CollisionTags.ChargeFuel, OnTargetHit);
		m_collisionDetector.RegisterListener(CollisionTags.Missile, OnTargetHit);

		m_rigidbody = GetComponent<Rigidbody>();
		IsAlive = false;
	}

	public void Spawn(Vector3 position) {
		IsAlive = true;

		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		m_trail.Stop();
	
		transform.position = position;
		transform.rotation = new Quaternion();;

		m_maxSpeed = (float) MathHelper.Random.NextDouble() * 1.5f + 2.75f;
		m_maxRotationSpeed = (float) MathHelper.Random.NextDouble() * 0.5f + 1.25f;

		m_target = BattleContext.PlayerShip.Position;

		StartCoroutine(LaunchProcess());
	}

	private IEnumerator LaunchProcess() {
		m_power = 1.1f;
		m_inactive = true;
		m_rigidbody.drag = 2.0f;
		GetComponent<Collider>().enabled = false;

		m_rigidbody.AddExplosionForce(340 * m_rigidbody.mass, transform.position + new Vector3(0, -0.04f, 0), 1);
		yield return new WaitForSeconds(0.75f);

		m_trail.Play();

		GetComponent<Collider>().enabled = true;
		m_rigidbody.drag = 0.0f;
		m_inactive = false;

		m_target = BattleContext.PlayerShip.Position;
	}

	private void OnTargetHit(GameObject other) {
		BattleContext.ExplosionsController.RocketExplosion(transform.position);
		IsAlive = false;
	}

	public void UpdateBullet() {
		if (m_inactive) {
			return;
		}

		Vector3 angularVelocityError = m_rigidbody.angularVelocity * -1;         
        Vector3 angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.fixedDeltaTime);
 
        m_rigidbody.AddTorque(angularVelocityCorrection * 1.0f);
 
        Vector3 desiredHeading = m_target - transform.position; 
        Vector3 currentHeading = transform.up;
 
        Vector3 headingError = Vector3.Cross(currentHeading, desiredHeading);
        Vector3 headingCorrection = headingController.Update(headingError, Time.fixedDeltaTime);
 
        m_rigidbody.AddTorque(headingCorrection * 1.2f);
		if (m_rigidbody.angularVelocity.magnitude > m_maxRotationSpeed) {
			m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * m_maxRotationSpeed;
		}

		if (m_power < 120) {
			m_power += Time.fixedDeltaTime * Mathf.Min(m_power * m_power * m_power, 50);
		}
		m_rigidbody.AddRelativeForce(0, m_power, 0);
		if (m_rigidbody.velocity.magnitude > m_maxSpeed) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxSpeed;
		}

		if (transform.position.y <= 0) {
			IsAlive = false;
			BattleContext.ExplosionsController.RocketExplosion(transform.position);
			BattleContext.EffectsController.SpawnSlowingCloud(transform.position);
		}
	}

	public bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
			if (!value) {
				m_trail.Stop();
			}
		}
	}

}
