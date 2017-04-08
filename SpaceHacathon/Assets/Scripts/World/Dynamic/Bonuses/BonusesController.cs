using System.Collections.Generic;
using UnityEngine;

public class BonusesController : IController {
	private SettingsBonusesController m_settings;
	
	private List<ChargeFuel> m_chargeFuels;

	public override void Initiate() {
		m_settings = BattleContext.Settings.BonusesController;

		m_chargeFuels = new List<ChargeFuel>();

		if (m_settings.EnableFuel) {
			for (int i = 0; i != m_settings.FuelCount; i++) {
				EntitiesHelper.CreateEntity(AvailablePrefabs.ChargeFuel, gameObject, m_chargeFuels);
			}
		}	}

	public override void FixedUpdateEntity() {
		for (int i = 0; i != m_chargeFuels.Count; i++) {
			if (!m_chargeFuels[i].IsSpawned()) {
				Respawn(m_chargeFuels[i]);
			} else {
				m_chargeFuels[i].FixedUpdateEntity();
			}
		}
	}

	private void Respawn(ChargeFuel fuel) {
		Vector3 playerPos = BattleContext.BattleManager.Director.PlayerShip.Position;
		fuel.Spawn(MathHelper.GetPointAround(playerPos, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.FuelSpawnAngle, m_settings.FuelSpawnMinDist, m_settings.FuelSpawnMaxDist), 0);
	}

	public List<ChargeFuel> ChargeFuels {
		get {
			return m_chargeFuels;
		}
	}

}
