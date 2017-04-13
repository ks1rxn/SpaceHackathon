using UnityEngine;

public class RocketShip : IEnemyShip {
	private SettingsRocketShip m_settings;

	private float m_globalCooldown;

	[SerializeField]
	private Transform m_launcher1;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.RocketShip;

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnPlayerShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnRamShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_globalCooldown = m_settings.LaunchCooldown;
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
		Vector3 enemyPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		if (Rigidbody.velocity.magnitude > 0) {
			Rigidbody.AddForce(-Rigidbody.velocity * 2);
		}

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - Position);
		if (Mathf.Abs(angle) > 10) {
			float angleSign = 0;
			if (!angle.Equals(0)) {
				angleSign = angle / Mathf.Abs(angle);
			}
			angleSign *= 10;
			Rigidbody.AddTorque(new Vector3(0, (angleSign - Rigidbody.angularVelocity.y * 50) * m_settings.RotationAcceleration * Time.fixedDeltaTime, 0));
			if (Rigidbody.angularVelocity.magnitude > m_settings.MaxRotationSpeed) {
				Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * m_settings.MaxRotationSpeed;
			}
		}

		if ((Mathf.Abs(angle) < m_settings.MaxLaunchAngle) && Vector3.Distance(BattleContext.BattleManager.Director.PlayerShip.Position, transform.position) < m_settings.MaxLaunchDistance) {
			if (m_globalCooldown <= 0) {
				//todo: create random launch points
				SpawnRocket1();
				m_globalCooldown = m_settings.LaunchCooldown;
			}
		}

		m_globalCooldown -= Time.fixedDeltaTime;
	}

	private void SpawnRocket1() {
		BattleContext.BattleManager.BulletsController.SpawnMissile(m_launcher1.position, transform.rotation.eulerAngles.y);

		Rigidbody.AddExplosionForce(120, m_launcher1.position, 3);
		m_launcher1.GetComponent<ParticleSystem>().Play();
		Rigidbody.AddTorque(0, 30, 0);
	}

	protected override float DistanceToDespawn {
		get {
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}

}
