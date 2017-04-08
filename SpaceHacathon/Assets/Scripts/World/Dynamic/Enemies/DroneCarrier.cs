using System;
using System.Collections;
using UnityEngine;

public class DroneCarrier : IEnemyShip {
	private SettingsDroneCarrier m_settings;

	private float m_cooldown;
	private float m_blasterTimerFront;
	private float m_blasterTimerRear;

	[SerializeField]
	private Transform m_launcher;
	[SerializeField]
	private GameObject m_frontGun;
	[SerializeField]
	private GameObject m_rearGun;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.DroneCarrier;

		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnRamShipHit);
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnPlayerShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		const float speed = 0.3f;
		Rigidbody.velocity = new Vector3(Mathf.Cos(angle.y * Mathf.PI / 180) * speed, 0, -Mathf.Sin(angle.y * Mathf.PI / 180) * speed);

		m_cooldown = m_settings.CarrierRocketCooldown;
	}

	protected override void OnDespawn(DespawnReason reason) {
		if (reason == DespawnReason.Kill) {
			BattleContext.BattleManager.ExplosionsController.ShipExplosion(Position);
		}
	}

	private void OnPlayerShipHit(GameObject other) {
		PlayerShip player = other.GetComponent<PlayerShip>();
		if (player != null && player.State == ShipState.InCharge) {
			Despawn(DespawnReason.Kill);
		}
	}

	private void OnRamShipHit(GameObject other) {
		RamShip ram = other.GetComponent<RamShip>();
		if (ram.State == RamShipState.Running) {
			Despawn(DespawnReason.Kill);
		}
	}

	protected override void OnFixedUpdateEntity() {
		UpdateGun(m_frontGun);
		UpdateGun(m_rearGun);
		UpdateLauncher();
	}

	private void UpdateGun(GameObject gun) {
		Vector3 playerPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
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
				m_blasterTimerFront = m_settings.BlasterCooldown;
			}
		} else {
			m_blasterTimerRear -= Time.deltaTime;
			if ((m_blasterTimerRear <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(playerPosition, Position) < 15) {
				StartCoroutine(GunFire(gun));
				m_blasterTimerRear = m_settings.BlasterCooldown;
			}
		}

	}

	private IEnumerator GunFire(GameObject gun) {
		for (int i = 0; i != 6; i++) {
			BattleContext.BattleManager.BulletsController.SpawnLaser(gun.transform.position, gun.transform.eulerAngles.y);
			float delay = (float)MathHelper.Random.NextDouble() * 0.10f;
			yield return new WaitForSeconds(0.05f + delay);
		}
	}

	private void UpdateLauncher() {
		if (Vector3.Distance(BattleContext.BattleManager.Director.PlayerShip.Position, Position) < 20) {
			if (m_cooldown <= 0) {
				SpawnRocket();
				m_cooldown = m_settings.CarrierRocketCooldown;
			}
		}

		m_cooldown -= Time.fixedDeltaTime;
	}

	private void SpawnRocket() {
		BattleContext.BattleManager.BulletsController.SpawnCarrierRocket(m_launcher.position);
	}

	protected override float DistanceToDespawn {
		get {
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}

}
