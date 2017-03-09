using UnityEngine;

public class StunProjectile : IBullet {
	private float m_angle;
	private float m_detonatorActivateTime;

	[SerializeField]
	private TrailRenderer m_trail1;
	[SerializeField]
	private TrailRenderer m_trail2;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterDefaultListener(OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_angle = angle.y;

		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
		m_trail1.Clear();
		m_trail2.Clear();
	}

	protected override void OnDespawn() {
		BattleContext.ExplosionsController.BlasterExplosion(transform.position);
	}

	protected override void OnFixedUpdateEntity() {
		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * 10 * Time.fixedDeltaTime;

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.Position, transform.position);
		if (distToPlayer > 20) {
			Despawn();
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.fixedDeltaTime;
		}
	}
	
	private void OnTargetHit(GameObject other) {
		Despawn();
	}

	protected override float DistanceToDespawn {
		get {
			return 40;
		}
	}
	
}
