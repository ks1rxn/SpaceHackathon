using UnityEngine;

public class Missile : IBullet {
	private SettingsMissile m_settings;

	private float m_lifeTime;
	private float m_detonatorActivateTime;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.Missile;

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.Missile, OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_lifeTime = m_settings.LifeTime;

		m_detonatorActivateTime = 0.1f;
		GetComponent<Collider>().enabled = false;

		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Rigidbody.AddForce(lookVector * 50);
	}

	protected override void OnDespawn(DespawnReason reason) {
		if (reason == DespawnReason.OutOfRange) {
			return;
		}
		BattleContext.BattleManager.ExplosionsController.RocketExplosion(transform.position);
	}

	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		Vector3 enemyPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		Rigidbody.AddTorque(new Vector3(0, (angle - Rigidbody.angularVelocity.y * 50) * m_settings.RotationAcceleration * Time.fixedDeltaTime, 0));
		if (Rigidbody.angularVelocity.magnitude > m_settings.MaxRotationSpeed) {
			Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * m_settings.MaxRotationSpeed;
		}

		Rigidbody.AddForce(lookVector.normalized * m_settings.Acceleration);
		if (Rigidbody.velocity.magnitude > m_settings.MaxSpeed) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * m_settings.MaxSpeed;
		}

		m_lifeTime -= Time.fixedDeltaTime;
		if (m_lifeTime <= 0) {
			Despawn(DespawnReason.TimeOff);
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.fixedDeltaTime;
		}
	}

	protected override float DistanceToDespawn {
		get {
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}

}
