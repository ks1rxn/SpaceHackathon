using System.Collections.Generic;
using UnityEngine;

public class HealthDroneStation : IAlly {
	[SerializeField]
	private GameObject m_healthDronePrefab;
	[SerializeField]
	private Transform[] m_dronePoints;

	private List<HealthDrone> m_drones; 

	protected override void OnPhysicBodyInitiate() {
		m_drones = new List<HealthDrone>();
		CreateHealthDrone();
		CreateHealthDrone();
		CreateHealthDrone();

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnPlayerEnter);
		CollisionDetector.RegisterExitListener(CollisionTags.PlayerShip, OnPlayerExit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_drones[0].Spawn(m_dronePoints[0].position, 0);
		m_drones[1].Spawn(m_dronePoints[1].position, 0);
		m_drones[2].Spawn(m_dronePoints[2].position, 0);
	}

	private void OnPlayerEnter(GameObject other) {

	}

	private void OnPlayerExit(GameObject other) {

	}


	protected override void OnDespawn() {
		foreach (HealthDrone drone in m_drones) {
			drone.Despawn();
		}
	}

	protected override void OnFixedUpdateEntity() {
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
