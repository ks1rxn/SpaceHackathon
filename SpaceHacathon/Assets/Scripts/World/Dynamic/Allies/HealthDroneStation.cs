using System.Collections.Generic;
using UnityEngine;

public class HealthDroneStation : IAlly {
	[SerializeField]
	private GameObject m_healthDronePrefab;
	[SerializeField]
	private Transform[] m_dronePoints;

	private List<HealthDrone> m_drones; 

	public override void Initiate() {
		base.Initiate();

		m_drones = new List<HealthDrone>();
		CreateHealthDrone();
		CreateHealthDrone();
		CreateHealthDrone();
	}

	public override void Spawn(Vector3 position, float rotation) {
		base.Spawn(position, rotation);
		m_drones[0].Spawn(m_dronePoints[0].position, 0);
		m_drones[1].Spawn(m_dronePoints[1].position, 0);
		m_drones[2].Spawn(m_dronePoints[2].position, 0);
	}

	protected override void OnDie() {
		foreach (HealthDrone drone in m_drones) {
			drone.Hide();
		}
	}

	public override void UpdateEntity() {
		base.UpdateEntity();
		foreach (HealthDrone drone in m_drones) {
			drone.UpdateEntity();
		}
	}

	protected override float DistanceFromPlayerToDie {
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
