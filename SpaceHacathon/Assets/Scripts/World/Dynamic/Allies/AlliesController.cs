using SpaceHacathon.Helpers;
using UnityEngine;

public class AlliesController : IController {
	private SettingsAlliesController m_settings;

	private HealthDroneStation m_activeStation;
	private HealthDroneStation m_disabledStation;

	public override void Initiate() {
		m_settings = BattleContext.Settings.AlliesController;
	
		m_activeStation = (HealthDroneStation) EntitiesHelper.CreateEntity(AvailablePrefabs.HealDroneStation, gameObject);
		m_disabledStation = (HealthDroneStation) EntitiesHelper.CreateEntity(AvailablePrefabs.HealDroneStation, gameObject);
	}

	public override void FixedUpdateEntity() {
		if (!m_activeStation.IsSpawned()) {
			Vector3 stationPosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.HealStationSpawnAngle, m_settings.HealStationSpawnMinDist, m_settings.HealStationSpawnMaxDist);
			m_activeStation.Spawn(stationPosition, 0);
		} else {
			m_activeStation.FixedUpdateEntity();
			if (m_activeStation.State == HealthDroneStationState.WorkDone) {
				Vector3 stationPosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.HealStationSpawnAngle, m_settings.HealStationSpawnMinDist, m_settings.HealStationSpawnMaxDist);
				m_disabledStation.Spawn(stationPosition, 0);
				HealthDroneStation temp = m_disabledStation;
				m_disabledStation = m_activeStation;
				m_activeStation = temp;
			}
		}
		if (m_disabledStation.IsSpawned()) {
			m_disabledStation.FixedUpdateEntity();
		}
	}

	public HealthDroneStation ActiveStation {
		get {
			return m_activeStation;
		}
	}

}
