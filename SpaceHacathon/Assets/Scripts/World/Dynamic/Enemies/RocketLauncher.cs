using System;
using System.Collections;
using UnityEngine;

public class RocketLauncher : IEnemyShip {
	private float m_cooldown;
	private float m_blasterTimerFront;
	private float m_blasterTimerRear;

	[SerializeField]
	private CollisionDetector m_collisionDetector;

	[SerializeField]
	private float m_globalCooldownValue;
	[SerializeField]
	private float m_blasterCooldown;

	[SerializeField]
	private Transform m_launcher;
	[SerializeField]
	private GameObject m_frontGun;
	[SerializeField]
	private GameObject m_rearGun;

	public override void Initiate() {
		base.Initiate();

		m_collisionDetector.Initiate();
		m_collisionDetector.RegisterListener("Player", OnOtherShipHit);
		m_collisionDetector.RegisterListener("RocketLauncherShip", OnOtherShipHit);
		m_collisionDetector.RegisterListener("StunShip", OnOtherShipHit);
		m_collisionDetector.RegisterListener("RamShip", OnOtherShipHit);
		m_collisionDetector.RegisterListener("SpaceMine", OnOtherShipHit);
	}

	public override void Spawn(Vector3 position, float angle) {
		base.Spawn(position, angle);

		const float speed = 0.3f;
		m_rigidbody.velocity = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) * speed, 0, -Mathf.Sin(angle * Mathf.PI / 180) * speed);

		m_cooldown = 0;
	}

	public override void Kill() {
		base.Kill();
		BattleContext.ExplosionsController.PlayerShipExplosion(Position);
	}

	private void OnOtherShipHit(GameObject other) {
		Kill();
	}

	public override void UpdateShip() {
		base.UpdateShip();
		
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
		float angleToTarget = MathHelper.AngleBetweenVectors(gunDirection, playerPosition - transform.position);

		if (angleToTarget > 5) {
			gun.transform.Rotate(0, 3, 0);
		} else if (angleToTarget < -5) {
			gun.transform.Rotate(0, -3, 0);
		}

		m_blasterTimerFront -= Time.deltaTime;
		if ((m_blasterTimerFront <= 0) && (Mathf.Abs(angleToTarget) < 10) && Vector3.Distance(playerPosition, Position) < 15) {
			StartCoroutine(GunFire(gun));
			m_blasterTimerFront = m_blasterCooldown;
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
		BattleContext.BulletsController.SpawnRocket(m_launcher.position);
	}

	public override bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
		}
	}

	protected override float DistanceFromPlayerToDie {
		get {
			return 40;
		}
	}

}
