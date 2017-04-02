using System.Collections.Generic;
using UnityEngine;

public class AlliesController : IController {
	private SettingsAlliesController m_settings;

	[SerializeField]
	private GameObject m_healthDroneStationPrefab;

	private HealthDroneStation m_activeStation;
	private HealthDroneStation m_disabledStation;

	private List<IAlly> m_allies;

	public override void Initiate() {
		m_settings = BattleContext.Settings.AlliesController;

		m_allies = new List<IAlly>();

		m_activeStation = CreateHealthDroneStation();
		m_disabledStation = CreateHealthDroneStation();
	}

	public override void FixedUpdateEntity() {
		if (!m_activeStation.IsSpawned()) {
			Vector3 stationPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_settings.HealStationSpawnAngle, m_settings.HealStationSpawnMinDist, m_settings.HealStationSpawnMaxDist);
			m_activeStation.Spawn(stationPosition, 0);
		} else {
			m_activeStation.FixedUpdateEntity();
			if (m_activeStation.State == HealthDroneStationState.WorkDone) {
				Vector3 stationPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_settings.HealStationSpawnAngle, m_settings.HealStationSpawnMinDist, m_settings.HealStationSpawnMaxDist);
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

	private HealthDroneStation CreateHealthDroneStation() {
		HealthDroneStation station = Instantiate(m_healthDroneStationPrefab).GetComponent<HealthDroneStation>();
		station.transform.parent = transform;
		station.Initiate();
		return station;
	}

}
