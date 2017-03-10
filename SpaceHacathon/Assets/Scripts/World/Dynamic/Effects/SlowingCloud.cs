using UnityEngine;

public class SlowingCloud : IEffect{
	private float m_lifeTime;

	protected override void OnInitiate() {
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		m_lifeTime = 5;
	}

	protected override void OnDespawn(DespawnReason reason) {
	}

	protected override void OnFixedUpdateEntity() {
		m_lifeTime -= Time.fixedDeltaTime;
		if (m_lifeTime <= 0) {
			Despawn(DespawnReason.TimeOff);
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 80;
		}
	}

}
