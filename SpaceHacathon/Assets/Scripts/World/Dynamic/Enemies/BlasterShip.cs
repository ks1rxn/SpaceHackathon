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
	[SerializeField]
	private ParticleSystem[] m_engines;
	private bool[] m_engineState;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
		m_engineState = new bool[m_engines.Length];
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
					Vector3 forcesSumm = new Vector3();
					float distance = (enemyPosition - transform.position).magnitude;
					if (distance > 6) {
						forcesSumm += -(transform.position - enemyPosition).normalized * m_rigidbody.mass * 10;
					} else {
						forcesSumm += (transform.position - enemyPosition).normalized * m_rigidbody.mass * 10;
					}
					foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
						if (ReferenceEquals(this, ship)) {
							continue;
						}
						if (Vector3.Distance(ship.Position, Position) > 5) {
							continue;
						}
						forcesSumm += (Position - ship.Position).normalized * m_rigidbody.mass * 75 / Vector3.Distance(ship.Position, Position);
					}
					m_rigidbody.AddForce(forcesSumm);
					if (m_rigidbody.velocity.magnitude > 7.5) {
						m_rigidbody.velocity = m_rigidbody.velocity.normalized * 7.5f;
					}
					if (forcesSumm.magnitude > 0.5f) {
						if (forcesSumm.x > 0) {
							if (forcesSumm.z < 0) {
								SetEngineState(0, false);
								SetEngineState(1, false);
								SetEngineState(2, true);
								SetEngineState(3, false);
							} else {
								SetEngineState(0, false);
								SetEngineState(1, false);
								SetEngineState(2, false);
								SetEngineState(3, true);
							}
						} else {
							if (forcesSumm.z < 0) {
								SetEngineState(0, false);
								SetEngineState(1, true);
								SetEngineState(2, false);
								SetEngineState(3, false);
							} else {
								SetEngineState(0, true);
								SetEngineState(1, false);
								SetEngineState(2, false);
								SetEngineState(3, false);
							}
						}
					} else {
						SetEngineState(0, false);
						SetEngineState(1, false);
						SetEngineState(2, false);
						SetEngineState(3, false);
					}
				} else {
					m_state = BlasterShipState.Refuel;
					for (int i = 0; i != 4; i++) {
						SetEngineState(i, false);
					}
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
		if ((m_blasterTimer <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) < 15 && !HittingAlly(gunDirection)) {
			BattleContext.BulletsController.SpawnBlaster(m_gun.transform.position, m_gun.transform.eulerAngles.y);
			m_blasterTimer = m_blasterCooldown;
		}
	}

	private bool HittingAlly(Vector3 gunDirection) {
		float disntaceToPlayer = Vector3.Distance(Position, BattleContext.PlayerShip.transform.position);
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (ReferenceEquals(ship, this)) {
				continue;
			}
			if (Vector3.Distance(ship.Position, Position) > disntaceToPlayer * 2) {
				continue;
			}
			float distance = DistanceToLine(Position, Position + gunDirection, ship.Position);
			if (distance < 1.25f) {
				return true;
			}
		}
		return false;
	}

	private float DistanceToLine(Vector3 p1, Vector3 p2, Vector3 point) {
		return Mathf.Abs((p2.z - p1.z) * point.x - (p2.x - p1.x) * point.z + p2.x * p1.z - p2.z * p1.x) / Mathf.Sqrt(Mathf.Pow(p2.z - p1.z, 2) + Mathf.Pow(p2.x - p1.x, 2));
	}

	private void SetEngineState(int engine, bool state) {
		if (m_engineState[engine] != state) {
			m_engineState[engine] = state;
			if (state) {
				m_engines[engine].Play();
			} else {
				m_engines[engine].Stop();
			}
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
