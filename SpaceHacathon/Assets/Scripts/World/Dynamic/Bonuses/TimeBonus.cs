using SpaceHacathon.Helpers;
using UnityEngine;

public class TimeBonus : IBonus {
	private SettingsTimeBonus m_settings;

	private float m_giveSeconds;

	protected override void OnInitiate() {
		m_settings = BattleContext.Settings.TimeBonus;

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		m_giveSeconds = MathHelper.Random.Next(m_settings.GiveSecondsDispertion * 2) - m_settings.GiveSecondsDispertion + m_settings.GiveSecondsValue;
	}

	protected override void OnDespawn(DespawnReason reason) {
	}

	protected override void OnFixedUpdateEntity() {
	}

	private void OnTargetHit(GameObject other) {
//		BattleContext.BattleManager.BonusesController.RespawnTimeBonus(this);
	}

	protected override float DistanceToDespawn {
		get {
			return 1000;
		}
	}

	public float GiveSeconds {
		get {
			return m_giveSeconds;
		}
	}

}
