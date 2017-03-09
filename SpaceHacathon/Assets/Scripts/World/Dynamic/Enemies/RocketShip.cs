using System;
using UnityEngine;

public class RocketShip : IEnemyShip {
	private float m_gun1Cooldown;
	private float m_gun2Cooldown;
	private float m_globalCooldown;

	[SerializeField]
	private float m_gunCooldownValue;
	[SerializeField]
	private float m_globalCooldownValue;

	[SerializeField]
	private Transform m_launcher1;
	[SerializeField]
	private Transform m_launcher2;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.SpaceMine, OnOtherShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_gun1Cooldown = 0;
		m_gun2Cooldown = 0;
		m_globalCooldown = m_globalCooldownValue;
	}

	protected override void OnDespawn() {
		BattleContext.ExplosionsController.PlayerShipExplosion(Position);
	}

	private void OnOtherShipHit(GameObject other) {
		Despawn();
	}

	protected override void OnFixedUpdateEntity() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		if (Rigidbody.velocity.magnitude > 0) {
			Rigidbody.AddForce(-Rigidbody.velocity * 2);
		}

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		if (Mathf.Abs(angle) > 10) {
			float angleSign = 0;
			if (!angle.Equals(0)) {
				angleSign = angle / Mathf.Abs(angle);
			}
			angleSign *= 10;
			Rigidbody.AddTorque(new Vector3(0, (angleSign - Rigidbody.angularVelocity.y * 50) * 0.1f, 0));
			if (Rigidbody.angularVelocity.magnitude > 1f) {
				Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * 1f;
			}
		}

		if ((Mathf.Abs(angle) < 45) && Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) < 20) {
			if ((m_gun1Cooldown <= 0) && (m_globalCooldown <= 0)) {
				SpawnRocket1();
				m_gun1Cooldown = m_gunCooldownValue;
				m_globalCooldown = m_globalCooldownValue;
			} else if ((m_gun2Cooldown <= 0) && (m_globalCooldown <= 0)) {
				SpawnRocket2();
				m_gun2Cooldown = m_gunCooldownValue;
				m_globalCooldown = m_globalCooldownValue;
			}
		}

		m_gun1Cooldown -= 0.02f;
		m_gun2Cooldown -= 0.02f;
		m_globalCooldown -= 0.02f;
	}

	private void SpawnRocket1() {
		BattleContext.BulletsController.SpawnMissile(m_launcher1.position, transform.rotation.eulerAngles.y);

		Rigidbody.AddExplosionForce(120, m_launcher1.position, 3);
		m_launcher1.GetComponent<ParticleSystem>().Play();
		Rigidbody.AddTorque(0, 30, 0);
	}

	private void SpawnRocket2() {
		BattleContext.BulletsController.SpawnMissile(m_launcher2.position, transform.rotation.eulerAngles.y);

		Rigidbody.AddExplosionForce(120, m_launcher2.position, 3);
		m_launcher2.GetComponent<ParticleSystem>().Play();
		Rigidbody.AddTorque(0, -30, 0);
	}

	protected override float DistanceToDespawn {
		get {
			return 40;
		}
	}

}
