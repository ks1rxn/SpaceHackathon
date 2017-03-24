using UnityEngine;

public class Laser : IBullet {
	private float m_angle;
	private float m_detonatorActivateTime;
	private float m_lifetime;

	[SerializeField]
	private TrailRenderer m_trail;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_angle = angle.y;
		m_trail.Clear();

		m_lifetime = 0.5f + (float)MathHelper.Random.NextDouble() * 0.2f;
		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.BlasterExplosion(transform.position);
	}

	protected override void OnFixedUpdateEntity() {
		m_lifetime -= Time.fixedDeltaTime;
		if (m_lifetime < 0) {
			Despawn(DespawnReason.TimeOff);
		}

		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * 20 * Time.fixedDeltaTime;

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.fixedDeltaTime;
		}
	}

	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override float DistanceToDespawn {
		get {
			return 40;
		}
	}
	
}
