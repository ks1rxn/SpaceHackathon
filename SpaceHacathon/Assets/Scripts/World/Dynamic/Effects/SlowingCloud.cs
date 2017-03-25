using UnityEngine;

public class SlowingCloud : IEffect {
	private SettingsSlowingCloud m_settings;
	private float m_lifeTime;

	protected override void OnInitiate() {
		m_settings = BattleContext.Settings.SlowingCloud;
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		m_lifeTime = m_settings.LifeTime;
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
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}

}
