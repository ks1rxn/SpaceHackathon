using System.Collections.Generic;
using UnityEngine;

public class HealthDroneStation : IAlly {
	private SettingsHealingDroneStation m_settings;

	[SerializeField]
	private GameObject m_healthDronePrefab;
	[SerializeField]
	private Transform[] m_dronePoints;
	[SerializeField]
	private GameObject m_sleepZone;
	[SerializeField]
	private GameObject m_activeZone;

	private HealthDroneStationState m_state;
	private List<HealthDrone> m_drones;
	private float m_healthLeft;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.HealingDroneStation;

		m_drones = new List<HealthDrone>();
		CreateHealthDrone();
		CreateHealthDrone();
		CreateHealthDrone();

		m_sleepZone.transform.localScale = new Vector3(m_settings.HealingRadius / 2, m_settings.HealingRadius / 2, m_settings.HealingRadius / 2);
		m_activeZone.transform.localScale = new Vector3(m_settings.HealingRadius / 2, m_settings.HealingRadius / 2, m_settings.HealingRadius / 2);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		ToSleepState();
	}

	protected override void OnDespawn(DespawnReason reason) {
		foreach (HealthDrone drone in m_drones) {
			drone.Despawn(DespawnReason.ParentDespawn);
		}
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case HealthDroneStationState.Sleep:
				PerformSleepState();
				break;
			case HealthDroneStationState.HealPlayer:
				PerformHealPlayerState();
				break;
			case HealthDroneStationState.WorkDone:
				PerformWorkDoneState();
				break;
		}

		foreach (HealthDrone drone in m_drones) {
			drone.FixedUpdateEntity();
		}
	}

	private void ToSleepState() {
		m_state = HealthDroneStationState.Sleep;

		m_drones[0].Spawn(m_dronePoints[0].position, 0);
		m_drones[0].SetBase(m_dronePoints[0]);

		m_drones[1].Spawn(m_dronePoints[1].position, 0);
		m_drones[1].SetBase(m_dronePoints[1]);

		m_drones[2].Spawn(m_dronePoints[2].position, 0);
		m_drones[2].SetBase(m_dronePoints[2]);

		m_healthLeft = m_settings.TotalHealthCapacity;
		m_activeZone.SetActive(false);
		m_sleepZone.SetActive(true);
	}

	private void PerformSleepState() {
		Vector3 playerPosition = BattleContext.Director.PlayerPosition;
		if (Vector3.Distance(playerPosition, Position) < m_settings.HealingRadius - 0.05f) {
			ToHealPlayerState();
		}
	}

	private void ToHealPlayerState() {
		m_state = HealthDroneStationState.HealPlayer;

		foreach (HealthDrone drone in m_drones) {
			drone.ToMoveToPlayerState();
		}

		m_activeZone.SetActive(true);
	}

	private void PerformHealPlayerState() {
		Vector3 playerPosition = BattleContext.Director.PlayerPosition;
		if (Vector3.Distance(playerPosition, Position) > m_settings.HealingRadius + 0.05f) {
			ToWorkDoneState();
		}

		if (m_healthLeft <= 0) {
			ToWorkDoneState();
		}

		float hp = Time.fixedDeltaTime * m_settings.TotalHealthCapacity / m_settings.TimeToGiveTotalHealth;
		m_healthLeft -= hp;
		BattleContext.PlayerShip.OnHeal(hp);
	}

	private void ToWorkDoneState() {
		m_state = HealthDroneStationState.WorkDone;

		foreach (HealthDrone drone in m_drones) {
			drone.ToMoveToBaseState();
		}

		m_activeZone.SetActive(false);
		m_sleepZone.SetActive(false);
	}

	private void PerformWorkDoneState() {
		
	}

	protected override float DistanceToDespawn {
		get {
			return m_settings.DistanceFromPlayerToDespawn;
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
	Sleep,
	HealPlayer,
	WorkDone
}
