using System.Collections.Generic;
using UnityEngine;

public class AlliesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_healthDroneStationPrefab;

	private List<IAlly> m_allies;

	public void Initiate() {
		m_allies = new List<IAlly>();

		for (int i = 0; i != 1; i++) {
			CreateHealthDroneStation();
		}
	}

	public void UpdateEntity() {
		for (int i = 0; i != m_allies.Count; i++) {
			if (m_allies[i].IsAlive) {
				m_allies[i].UpdateEntity();
			} else {
				if (m_allies[i] is HealthDroneStation) {
					Vector3 stationPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, 30, 50);
					SpawnHealthDroneStation(stationPosition, 0);
				}
			}
		}
	}

	public HealthDroneStation SpawnHealthDroneStation(Vector3 position, float angle) {
		HealthDroneStation targetEntity = null;
		foreach (IAlly entity in m_allies) {
			if (entity is HealthDroneStation && !entity.IsAlive) {
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
