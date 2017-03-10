using System.Collections;
using UnityEngine;

public class StunShip : IEnemyShip {
	[SerializeField]
	private GameObject m_gun;

	[SerializeField]
	private ParticleSystem m_spawnEffect;
	private Material m_material;

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
	private ParticleSystem[] m_engines;
	private bool[] m_engineState;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.SpaceMine, OnOtherShipHit);

		m_material = GetComponent<MeshRenderer>().material;
		m_engineState = new bool[m_engines.Length];
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_blasterTimer = m_blasterCooldown;
		m_state = BlasterShipState.Moving;
		m_movingTimer = m_flyCooldown;

		StartCoroutine(SpawnEffect());
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.PlayerShipExplosion(Position);
		BattleContext.EnemiesController.OnStunShipDie();
	}

	private IEnumerator SpawnEffect() {
		m_spawnEffect.Play();
		float value = 1.0f;
		while (value > 0) {
			value -= Time.deltaTime;
			m_material.SetFloat("_SliceAmount", value);
			yield return new WaitForEndOfFrame();
		}
		m_material.SetFloat("_SliceAmount", 0.0f);
	}

	private void OnOtherShipHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case BlasterShipState.Moving:
				if (m_movingTimer > 0) {
					Vector3 enemyPosition = BattleContext.PlayerShip.Position;
					Vector3 forcesSumm = new Vector3();
					float distance = (enemyPosition - transform.position).magnitude;
					if (distance > 6) {
						forcesSumm += -(transform.position - enemyPosition).normalized * Rigidbody.mass * 10;
					} else {
						forcesSumm += (transform.position - enemyPosition).normalized * Rigidbody.mass * 20;
					}
					foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
						if (ReferenceEquals(this, ship)) {
							continue;
						}
						if (Vector3.Distance(ship.Position, Position) > 8) {
							continue;
						}
						forcesSumm += (Position - ship.Position).normalized * Rigidbody.mass * 75 / Vector3.Distance(ship.Position, Position);
					}
					foreach (ChargeFuel chargeFuel in BattleContext.BonusesController.ChargeFuels) {
						if (Vector3.Distance(chargeFuel.transform.position, Position) > 5) {
							continue;
						}
						forcesSumm += (Position - chargeFuel.transform.position).normalized * Rigidbody.mass * 75 / Vector3.Distance(chargeFuel.transform.position, Position);
					}
					Rigidbody.AddForce(forcesSumm);
					if (Rigidbody.velocity.magnitude > 7.5) {
						Rigidbody.velocity = Rigidbody.velocity.normalized * 7.5f;
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
					if (Rigidbody.velocity.magnitude > 0.5f) {
						Rigidbody.velocity = Rigidbody.velocity * 0.5f;
					} else {
						Rigidbody.velocity = new Vector3();
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
		Vector3 enemyPosition = BattleContext.PlayerShip.Position;
		Vector3 gunDirection = new Vector3(Mathf.Cos(-m_gun.transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-m_gun.transform.eulerAngles.y * Mathf.PI / 180));
		float angleToTarget = MathHelper.AngleBetweenVectors(gunDirection, enemyPosition - transform.position);

		if (angleToTarget > 5) {
			m_gun.transform.Rotate(0, 3, 0);
		} else if (angleToTarget < -5) {
			m_gun.transform.Rotate(0, -3, 0);
		}

		m_blasterTimer -= Time.deltaTime;
		if ((m_blasterTimer <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(BattleContext.PlayerShip.Position, transform.position) < 15 && !HittingAlly(gunDirection)) {
			BattleContext.BulletsController.SpawnStunProjectile(m_gun.transform.position, m_gun.transform.eulerAngles.y);
			m_blasterTimer = m_blasterCooldown;
		}
	}

	private bool HittingAlly(Vector3 gunDirection) {
		float disntaceToPlayer = Vector3.Distance(Position, BattleContext.PlayerShip.Position);
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

	protected override float DistanceToDespawn {
		get {
			return 50;
		}
	}

}

public enum BlasterShipState {
	Moving = 0,
	Refuel = 1
}
