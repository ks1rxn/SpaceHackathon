using UnityEngine;

public class BlasterShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private Transform m_parent;

	[SerializeField]
	private GameObject m_blasterPrefab;
	[SerializeField]
	private GameObject m_gun;

	private float m_gunAngle = 0;
	private float m_liveTime;
	private float m_blastCooldown;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, Transform parent) {
		m_parent = parent;
		transform.position = position;
		m_blastCooldown = 2;
		m_liveTime = 10;
	}

	protected void Update() {
		if (m_liveTime > 0) {
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
			if (m_rigidbody.velocity.magnitude > 0.5f) {
				m_rigidbody.velocity = m_rigidbody.velocity - m_rigidbody.velocity * 0.5f * Time.deltaTime;
			} else {
				m_rigidbody.velocity = new Vector3();
			}
		}

		m_liveTime -= Time.deltaTime;
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

		m_blastCooldown -= Time.deltaTime;
		if ((m_blastCooldown <= 0) && (Mathf.Abs(angle) < 10)) {
			SpawnBlast();
			m_blastCooldown = 2f;
		}
	}

	private void SpawnBlast() {
		Blaster blaster = ((GameObject)Instantiate(m_blasterPrefab)).GetComponent<Blaster>();
		blaster.transform.parent = m_parent;
		blaster.Spawn(m_gun.transform.position, m_gunAngle);
	}

}
