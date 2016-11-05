using UnityEngine;

public class BlasterShip : MonoBehaviour, IEnemyShip {
	private Rigidbody m_rigidbody;

	[SerializeField]
	private GameObject m_gun;

	private BlasterShipState m_state;

	private float m_movingTimer;
	private float m_blasterTimer;

	[SerializeField]
	private float m_flyCooldown;
	[SerializeField]
	private float m_refuelCooldown;
	[SerializeField]
	private float m_blasterCooldown;

	[SerializeField]
	private GameObject m_chargeTarget;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, float angle) {
		transform.position = position;

		m_blasterTimer = m_blasterCooldown;
		m_state = BlasterShipState.Moving;
		m_movingTimer = m_flyCooldown;

		UncheckAsTarget();
	}

	public void Kill() {
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
		Die();
	}

	private void Die() {
		BattleContext.EnemiesController.Respawn(this);
	}

	protected void OnCollisionEnter(Collision collision) {
		Kill();
	}

	public void CheckAsTarget() {
		m_chargeTarget.SetActive(true);
	}

	public void UncheckAsTarget() {
		m_chargeTarget.SetActive(false);
	}

	protected void FixedUpdate() {
		switch (m_state) {
			case BlasterShipState.Moving:
				if (m_movingTimer > 0) {
					Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
					float distance = (enemyPosition - transform.position).magnitude;
					if (distance > 6) {
						m_rigidbody.AddForce(-(transform.position - enemyPosition).normalized * m_rigidbody.mass * 10);
					} else {
						m_rigidbody.AddForce((transform.position - enemyPosition).normalized * m_rigidbody.mass * 10);
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
						m_rigidbody.velocity = m_rigidbody.velocity * 0.5f;
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

		if (Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) > 80) {
			Die();
		}
	}

	private void UpdateGun() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 gunDirection = new Vector3(Mathf.Cos(-m_gun.transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-m_gun.transform.eulerAngles.y * Mathf.PI / 180));
		float angleToTarget = MathHelper.AngleBetweenVectors(gunDirection, enemyPosition - transform.position);

		if (angleToTarget > 5) {
			m_gun.transform.Rotate(0, 3, 0);
		} else if (angleToTarget < -5) {
			m_gun.transform.Rotate(0, -3, 0);
		}

		m_blasterTimer -= Time.deltaTime;
		if ((m_blasterTimer <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) < 15) {
			BattleContext.BulletsController.SpawnBlaster(m_gun.transform.position, m_gun.transform.eulerAngles.y);
			m_blasterTimer = m_blasterCooldown;
		}
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}

public enum BlasterShipState {
	Moving = 0,
	Refuel = 1
}
