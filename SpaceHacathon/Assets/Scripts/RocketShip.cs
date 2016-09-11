using UnityEngine;

public class RocketShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private Transform m_parent;

	[SerializeField]
	private GameObject m_rocketPrefab;

	private float m_timeToLaunch1 = 2;
	private float m_timeToLaunch2 = 2.5f;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
//		m_rigidbody.centerOfMass = new Vector3(1, 0, 0);
	}

	public void Spawn(Vector3 position, Transform parent) {
		m_parent = parent;
		transform.position = position;
	}

	protected void Update() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		float angleSign = 0;
		if (angle != 0) {
			angleSign = angle / Mathf.Abs(angle);
		}
		angleSign *= 10;
		m_rigidbody.AddTorque(new Vector3(0, (angleSign - m_rigidbody.angularVelocity.y * 50) * 0.05f, 0));
		if (m_rigidbody.angularVelocity.magnitude > 1f) {
			m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 1f;
		}

		m_timeToLaunch1 -= Time.deltaTime;
		if ((m_timeToLaunch1 <= 0) && (Mathf.Abs(angle) < 45)) {
			SpawnRocket();
			m_timeToLaunch1 = 10;
//			m_timeToLaunch2 = 0.5f;
		}
//		m_timeToLaunch2 -= Time.deltaTime;
//		if ((m_timeToLaunch2 <= 0) && (angle < 45)) {
//			SpawnRocket();
//			m_timeToLaunch2 = 10;
//			m_timeToLaunch1 = 0.5f;
//		}
	}

	private void SpawnRocket() {
		Rocket rocket = ((GameObject)Instantiate(m_rocketPrefab)).GetComponent<Rocket>();
		rocket.transform.parent = m_parent;
		rocket.transform.rotation = transform.rotation;
		rocket.Spawn(transform.position, 10);
	}

}
