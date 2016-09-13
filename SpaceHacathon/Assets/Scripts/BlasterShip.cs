using UnityEngine;

public class BlasterShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private Transform m_parent;

	[SerializeField]
	private GameObject m_blasterPrefab;
	[SerializeField]
	private GameObject m_gun;

	private BlasterShipState m_state;

	private float m_gunAngle = 0;
	private float m_movingTimer;
	private float m_blasterTimer;

	[SerializeField]
	private float m_flyCooldown;
	[SerializeField]
	private float m_refuelCooldown;
	[SerializeField]
	private float m_blasterCooldown;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, Transform parent) {
		m_parent = parent;
		transform.position = position;

		m_blasterTimer = m_blasterCooldown;
		m_state = BlasterShipState.Moving;
		m_movingTimer = m_flyCooldown;
	}

	protected void Update() {
		switch (m_state) {
			case BlasterShipState.Moving:
				if (m_movingTimer > 0) {
					Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
					float distance = (enemyPosition - transform.position).magnitude;
					if (distance > 6) {
						m_rigidbody.AddForce(-(transform.position - enemyPosition).normalized * 10);
					} else {
						m_rigidbody.AddForce((transform.position - enemyPosition).normalized * 10);
					}
					if (m_rigidbody.velocity.magnitude > 10) {
						m_rigidbody.velocity = m_rigidbody.velocity.normalized * 10;
					}
				} else {
					m_state = BlasterShipState.Refuel;
					m_movingTimer = m_refuelCooldown;
				}
				break;
			case BlasterShipState.Refuel:
				if (m_movingTimer > 0) {
					if (m_rigidbody.velocity.magnitude > 0.5f) {
						m_rigidbody.velocity = m_rigidbody.velocity - m_rigidbody.velocity * 0.5f * Time.deltaTime;
					} else {
						m_rigidbody.velocity = new Vector3();
					}
				} else {
					m_state = BlasterShipState.Moving;
					m_movingTimer = m_flyCooldown;
				}
				break;
		}

		m_movingTimer -= Time.deltaTime;
		UpdateGun();
	}

	private void UpdateGun() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		float angle = MathHelper.AngleBetweenVectors(new Vector3(Mathf.Cos(-m_gunAngle * Mathf.PI / 180), 0, Mathf.Sin(-m_gunAngle * Mathf.PI / 180)), enemyPosition - transform.position);

		if (Mathf.Abs(angle) > 5) {
			if (angle > 0) {
				m_gunAngle += Time.deltaTime * 50;
			} else {
				m_gunAngle -= Time.deltaTime * 50;
			}
		}

		m_gun.transform.rotation = new Quaternion();
		m_gun.transform.Rotate(new Vector3(0, 1, 0), m_gunAngle);

		m_blasterTimer -= Time.deltaTime;
		if ((m_blasterTimer <= 0) && (Mathf.Abs(angle) < 10) && Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) < 15) {
			SpawnBlast();
			m_blasterTimer = m_blasterCooldown;
		}
	}

	private void SpawnBlast() {
		Blaster blaster = ((GameObject)Instantiate(m_blasterPrefab)).GetComponent<Blaster>();
		blaster.transform.parent = m_parent;
		blaster.Spawn(m_gun.transform.position, m_gunAngle);
	}

}

public enum BlasterShipState {
	Moving = 0,
	Refuel = 1
}
