using UnityEngine;

public class Missile : MonoBehaviour {
	[SerializeField]
	private CollisionDetector m_collisionDetector;

	private Rigidbody m_rigidbody;
	private float m_lifeTime;
	private float m_detonatorActivateTime;

	public void Initiate() {
		m_collisionDetector.Initiate();
		m_collisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
		m_collisionDetector.RegisterListener(CollisionTags.RamShip, OnTargetHit);
		m_collisionDetector.RegisterListener(CollisionTags.ChargeFuel, OnTargetHit);
		m_collisionDetector.RegisterListener(CollisionTags.Missile, OnTargetHit);

		m_rigidbody = GetComponent<Rigidbody>();
		IsAlive = false;
	}

	public void Spawn(Vector3 position, float angle) {
		IsAlive = true;

		transform.position = position;
		m_lifeTime = 10;

		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 1, 0), angle);

		m_detonatorActivateTime = 0.2f;
		GetComponent<Collider>().enabled = false;

		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		m_rigidbody.AddForce(lookVector * 50);
	}

	private void OnTargetHit(GameObject other) {
		BattleContext.ExplosionsController.RocketExplosion(transform.position);
		IsAlive = false;
	}

	public void UpdateBullet() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		m_rigidbody.AddTorque(new Vector3(0, (angle - m_rigidbody.angularVelocity.y * 50) * 0.1f, 0));
		if (m_rigidbody.angularVelocity.magnitude > 5) {
			m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 5;
		}

		m_rigidbody.AddForce(lookVector.normalized * 80);
		if (m_rigidbody.velocity.magnitude > 6) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 6;
		}

		m_lifeTime -= 0.02f;
		if (m_lifeTime <= 0) {
			IsAlive = false;
			BattleContext.ExplosionsController.RocketExplosion(transform.position);
		}

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position);
		if (distToPlayer > 25) {
			IsAlive = false;
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= 0.02f;
		}
	}

	public bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
		}
	}

}
