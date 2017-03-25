using UnityEngine;

public class Missile : IBullet {
	private float m_lifeTime;
	private float m_detonatorActivateTime;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.Missile, OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_lifeTime = 10;

		m_detonatorActivateTime = 0.2f;
		GetComponent<Collider>().enabled = false;

		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Rigidbody.AddForce(lookVector * 50);
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.RocketExplosion(transform.position);
	}

	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		Rigidbody.AddTorque(new Vector3(0, (angle - Rigidbody.angularVelocity.y * 50) * 0.1f, 0));
		if (Rigidbody.angularVelocity.magnitude > 5) {
			Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * 5;
		}

		Rigidbody.AddForce(lookVector.normalized * 80);
		if (Rigidbody.velocity.magnitude > 6) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * 6;
		}

		m_lifeTime -= 0.02f;
		if (m_lifeTime <= 0) {
			Despawn(DespawnReason.TimeOff);
			BattleContext.ExplosionsController.RocketExplosion(transform.position);
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= 0.02f;
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 60;
		}
	}

}
