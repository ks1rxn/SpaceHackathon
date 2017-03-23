using System.Collections.Generic;
using UnityEngine;

public class HealthDroneStation : IAlly {
	private SettingsHealingDroneStation m_settings;

	[SerializeField]
	private GameObject m_healthDronePrefab;
	[SerializeField]
	private Transform[] m_dronePoints;

	private HealthDroneStationState m_state;
	private List<HealthDrone> m_drones; 

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.HealingDroneStation;

		m_drones = new List<HealthDrone>();
		CreateHealthDrone();
		CreateHealthDrone();
		CreateHealthDrone();

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnPlayerEnter);
		CollisionDetector.RegisterExitListener(CollisionTags.PlayerShip, OnPlayerExit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_drones[0].Spawn(m_dronePoints[0].position, 0);
		m_drones[0].SetBase(m_dronePoints[0]);

		m_drones[1].Spawn(m_dronePoints[1].position, 0);
		m_drones[1].SetBase(m_dronePoints[1]);

		m_drones[2].Spawn(m_dronePoints[2].position, 0);
		m_drones[2].SetBase(m_dronePoints[2]);
	}

	private void OnPlayerEnter(GameObject other) {
		foreach (HealthDrone drone in m_drones) {
			drone.ToMoveToPlayerState();
		}
	}

	private void OnPlayerExit(GameObject other) {
		foreach (HealthDrone drone in m_drones) {
			drone.ToMoveToBaseState();
		}
	}


	protected override void OnDespawn(DespawnReason reason) {
		foreach (HealthDrone drone in m_drones) {
			drone.Despawn(DespawnReason.ParentDespawn);
		}
	}

	protected override void OnFixedUpdateEntity() {
		Vector3 playerPosition = BattleContext.Director.PlayerPosition;
		if (Vector3.Distance(playerPosition, Position) < m_settings.HealingRadius) {
			
		} else {
			
		}

		foreach (HealthDrone drone in m_drones) {
			drone.FixedUpdateEntity();
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 60;
		}
	}

	private HealthDrone CreateHealthDrone() {
		HealthDrone healthDrone = (Instantiate(m_healthDronePrefab)).GetComponent<HealthDrone>();
		healthDrone.transform.parent = BattleContext.AlliesController.gameObject.transform;
		healthDrone.Initiate();
		m_drones.Add(healthDrone);
		return healthDrone;
	}

}

public enum HealthDroneStationState {
	
}
