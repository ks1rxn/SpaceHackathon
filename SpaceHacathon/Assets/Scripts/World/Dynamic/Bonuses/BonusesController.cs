using System.Collections.Generic;
using SpaceHacathon.Helpers;
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
		Vector3 spawnPosition = MathHelper.GetPointAround(playerPos, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.FuelSpawnAngle, m_settings.FuelSpawnMinDist, m_settings.FuelSpawnMaxDist);
		if (Vector3.Distance(BattleContext.BattleManager.AlliesController.ActiveStation.Position, spawnPosition) < BattleContext.Settings.HealingDroneStation.HealingRadius * 1.2f) {
			spawnPosition += (spawnPosition - BattleContext.BattleManager.AlliesController.ActiveStation.Position).normalized * BattleContext.Settings.HealingDroneStation.HealingRadius * 1.2f;
		}
		fuel.Spawn(spawnPosition, 0);
	}

	public List<ChargeFuel> ChargeFuels {
		get {
			return m_chargeFuels;
		}
	}

}
