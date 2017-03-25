using System.Collections.Generic;
using UnityEngine;

public class AlliesController : IController {
	private SettingsAlliesController m_settings;

	[SerializeField]
	private GameObject m_healthDroneStationPrefab;

	private List<IAlly> m_allies;

	public override void Initiate() {
		m_settings = BattleContext.Settings.AlliesController;

		m_allies = new List<IAlly>();

		if (m_settings.EnableHealStation) {
			CreateHealthDroneStation();
		}
	}

	public override void FixedUpdateEntity() {
		for (int i = 0; i != m_allies.Count; i++) {
			if (m_allies[i].IsSpawned()) {
				m_allies[i].FixedUpdateEntity();
			} else {
				if (m_allies[i] is HealthDroneStation) {
					Vector3 stationPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_settings.HealStationSpawnAngle, m_settings.HealStationSpawnMinDist, m_settings.HealStationSpawnMaxDist);
					SpawnHealthDroneStation(stationPosition, 0);
				}
			}
		}
	}

	public HealthDroneStation SpawnHealthDroneStation(Vector3 position, float angle) {
		HealthDroneStation targetEntity = null;
		foreach (IAlly entity in m_allies) {
			if (entity is HealthDroneStation && !entity.IsSpawned()) {
				targetEntity = (HealthDroneStation) entity;
				break;
			}
		}
		if (targetEntity == null) {
			targetEntity = CreateHealthDroneStation();
		}
		targetEntity.Spawn(position, angle);
		return targetEntity;
	}

	private HealthDroneStation CreateHealthDroneStation() {
		HealthDroneStation station = Instantiate(m_healthDroneStationPrefab).GetComponent<HealthDroneStation>();
		station.transform.parent = transform;
		station.Initiate();
		m_allies.Add(station);
		return station;
	}

}
