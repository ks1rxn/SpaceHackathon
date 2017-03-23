using System;
using System.Collections;
using UnityEngine;

public class DroneCarrier : IEnemyShip {
	private float m_cooldown;
	private float m_blasterTimerFront;
	private float m_blasterTimerRear;

	[SerializeField]
	private float m_globalCooldownValue;
	[SerializeField, Range(1.0f, 5.0f)]
	private float m_blasterCooldown;

	[SerializeField]
	private Transform m_launcher;
	[SerializeField]
	private GameObject m_frontGun;
	[SerializeField]
	private GameObject m_rearGun;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		const float speed = 0.3f;
		Rigidbody.velocity = new Vector3(Mathf.Cos(angle.y * Mathf.PI / 180) * speed, 0, -Mathf.Sin(angle.y * Mathf.PI / 180) * speed);

		m_cooldown = MathHelper.Random.Next((int)m_globalCooldownValue);
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.PlayerShipExplosion(Position);
	}

	private void OnOtherShipHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		UpdateGun(m_frontGun);
		UpdateGun(m_rearGun);
		UpdateLauncher();
	}

	private void UpdateGun(GameObject gun) {
		Vector3 playerPosition = BattleContext.PlayerShip.Position;
		if (Vector3.Distance(playerPosition, Position) > 20) {
			return;
		}
		Vector3 gunDirection = new Vector3(Mathf.Cos(-gun.transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-gun.transform.eulerAngles.y * Mathf.PI / 180));
		float angleToTarget = MathHelper.AngleBetweenVectors(gunDirection, playerPosition - Position);

		Vector3 angle = gun.transform.localEulerAngles;
		if (angleToTarget > 5) {
			angle.y += 3;
		} else if (angleToTarget < -5) {
			angle.y -= 3;
		}

		if (gun == m_frontGun) {
			if (angle.y > 90 && angle.y < 180) {
				angle.y = 90;
			}
			if (angle.y < 270 && angle.y > 180) {
				angle.y = 270;
			}
		}

		if (gun == m_rearGun) {
			if (angle.y < 90) {
				angle.y = 90;
			}
			if (angle.y > 270) {
				angle.y = 270;
			}
		}

		gun.transform.localEulerAngles = angle;

		if (gun == m_frontGun) {
			m_blasterTimerFront -= Time.deltaTime;
			if ((m_blasterTimerFront <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(playerPosition, Position) < 15) {
				StartCoroutine(GunFire(gun));
				m_blasterTimerFront = m_blasterCooldown;
			}
		} else {
			m_blasterTimerRear -= Time.deltaTime;
			if ((m_blasterTimerRear <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(playerPosition, Position) < 15) {
				StartCoroutine(GunFire(gun));
				m_blasterTimerRear = m_blasterCooldown;
			}
		}

	}

	private IEnumerator GunFire(GameObject gun) {
		for (int i = 0; i != 6; i++) {
			BattleContext.BulletsController.SpawnLaser(gun.transform.position, gun.transform.eulerAngles.y);
			float delay = (float)MathHelper.Random.NextDouble() * 0.10f;
			yield return new WaitForSeconds(0.05f + delay);
		}
	}

	private void UpdateLauncher() {
		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) < 20) {
			if (m_cooldown <= 0) {
				SpawnRocket();
				m_cooldown = m_globalCooldownValue;
			}
		}

		m_cooldown -= Time.fixedDeltaTime;
	}

	private void SpawnRocket() {
		BattleContext.BulletsController.SpawnCarrierRocket(m_launcher.position);
	}

	protected override float DistanceToDespawn {
		get {
			return 40;
		}
	}

}
