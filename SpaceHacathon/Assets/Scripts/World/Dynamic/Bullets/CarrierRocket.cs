using System.Collections;
using UnityEngine;

public class CarrierRocket : IBullet {
	private float m_detonatorActivateTime;

	[SerializeField]
	private ParticleSystem m_trail;

	private float m_power;
	private bool m_inactive;

	private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

	private float m_maxSpeed = 0;
	private float m_maxRotationSpeed = 0;

	private Vector3 m_target;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.ChargeFuel, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.Missile, OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_trail.Stop();

		m_maxSpeed = (float) MathHelper.Random.NextDouble() * 1.5f + 2.75f;
		m_maxRotationSpeed = (float) MathHelper.Random.NextDouble() * 0.5f + 1.25f;

		m_target = BattleContext.PlayerShip.Position;

		StartCoroutine(LaunchProcess());
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.RocketExplosion(transform.position);
		BattleContext.EffectsController.SpawnSlowingCloud(transform.position);
	}

	private IEnumerator LaunchProcess() {
		m_power = 1.1f;
		m_inactive = true;
		Rigidbody.drag = 2.0f;
		GetComponent<Collider>().enabled = false;

		Rigidbody.AddExplosionForce(340 * Rigidbody.mass, transform.position + new Vector3(0, -0.04f, 0), 1);
		yield return new WaitForSeconds(0.75f);

		m_trail.Play();

		GetComponent<Collider>().enabled = true;
		Rigidbody.drag = 0.0f;
		m_inactive = false;

		m_target = BattleContext.PlayerShip.Position;
	}

	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		if (m_inactive) {
			return;
		}

		Vector3 angularVelocityError = Rigidbody.angularVelocity * -1;         
        Vector3 angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.fixedDeltaTime);
 
        Rigidbody.AddTorque(angularVelocityCorrection * 1.0f);
 
        Vector3 desiredHeading = m_target - transform.position; 
        Vector3 currentHeading = transform.up;
 
        Vector3 headingError = Vector3.Cross(currentHeading, desiredHeading);
        Vector3 headingCorrection = headingController.Update(headingError, Time.fixedDeltaTime);
 
        Rigidbody.AddTorque(headingCorrection * 1.2f);
		if (Rigidbody.angularVelocity.magnitude > m_maxRotationSpeed) {
			Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * m_maxRotationSpeed;
		}

		if (m_power < 120) {
			m_power += Time.fixedDeltaTime * Mathf.Min(m_power * m_power * m_power, 50);
		}
		Rigidbody.AddRelativeForce(0, m_power, 0);
		if (Rigidbody.velocity.magnitude > m_maxSpeed) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * m_maxSpeed;
		}

		if (transform.position.y <= 0) {
			Despawn(DespawnReason.TargetReached);
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 40;
		}
	}

}
