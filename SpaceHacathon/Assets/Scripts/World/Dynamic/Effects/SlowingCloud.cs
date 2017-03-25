using UnityEngine;

public class SlowingCloud : IEffect {
	private SettingsSlowingCloud m_settings;
	private float m_lifeTime;

	[SerializeField]
	private Transform m_child;
	[SerializeField]
	private Transform m_childChild;
	[SerializeField]
	private SphereCollider m_collider;

	protected override void OnInitiate() {
		m_settings = BattleContext.Settings.SlowingCloud;

		m_child.localScale = new Vector3(m_settings.Radius, m_settings.Radius, m_settings.Radius);
		m_childChild.localScale = new Vector3(m_settings.Radius * 0.6f, m_settings.Radius * 0.6f, m_settings.Radius * 0.6f);
		m_collider.radius = m_settings.Radius * 5;
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
