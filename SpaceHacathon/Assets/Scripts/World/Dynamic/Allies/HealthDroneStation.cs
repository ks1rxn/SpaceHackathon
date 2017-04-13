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
	[SerializeField]
	private GameObject m_noSalvation;
	[SerializeField]
	private Animator m_animator;

	private HealthDroneStationState m_state;
	private List<HealthDrone> m_drones;
	private float m_healthLeft;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.HealingDroneStation;

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnPlayerShipHit);

		m_drones = new List<HealthDrone>();
		CreateHealthDrone();
		CreateHealthDrone();
		CreateHealthDrone();

		m_sleepZone.transform.localScale = new Vector3(m_settings.HealingRadius / 2, m_settings.HealingRadius / 2, m_settings.HealingRadius / 2);
		m_activeZone.transform.localScale = new Vector3(m_settings.HealingRadius / 2, m_settings.HealingRadius / 2, m_settings.HealingRadius / 2);
		m_noSalvation.transform.localScale = new Vector3(m_settings.HealingRadius / 2, m_settings.HealingRadius / 2, m_settings.HealingRadius / 2);
	}

	private void OnPlayerShipHit(GameObject other) {
		PlayerShip player = other.GetComponent<PlayerShip>();
		if (player != null && player.State == ShipState.InCharge) {
			ToWorkDoneState();
			Despawn(DespawnReason.Kill);
		}
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		ToSleepState();
	}

	protected override void OnDespawn(DespawnReason reason) {
		foreach (HealthDrone drone in m_drones) {
			drone.Despawn(DespawnReason.ParentDespawn);
		}
		if (reason == DespawnReason.Kill) {
			BattleContext.BattleManager.ExplosionsController.ShipExplosion(Position);
		}
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case HealthDroneStationState.Sleep:
				PerformSleepState();
				break;
			case HealthDroneStationState.NoSalvation:
				PerformNoSalvationState();
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

		m_animator.SetBool("Off", false);

		m_activeZone.SetActive(false);
		m_noSalvation.SetActive(false);
		m_sleepZone.SetActive(true);
	}

	private void PerformSleepState() {
		Vector3 playerPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
		if (Vector3.Distance(playerPosition, Position) < m_settings.HealingRadius - 0.05f) {
			if (BattleContext.BattleManager.Director.PlayerShip.Hull.Cargo > 0) {
				ToHealPlayerState();
			} else {
				ToNoSalvationState();
			}
		}
	}

	private void ToNoSalvationState() {
		m_state = HealthDroneStationState.NoSalvation;

		BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.NoSalvation++;
		m_noSalvation.SetActive(true);
	}

	private void PerformNoSalvationState() {
		Vector3 playerPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
		if (Vector3.Distance(playerPosition, Position) < m_settings.HealingRadius - 0.05f) {
			if (BattleContext.BattleManager.Director.PlayerShip.Hull.Cargo > 0) {
				ToHealPlayerState();
			}
		} else {
			ToSleepState();
		}
	}

	private void ToHealPlayerState() {
		m_state = HealthDroneStationState.HealPlayer;

		foreach (HealthDrone drone in m_drones) {
			drone.ToMoveToPlayerState();
		}

		m_healthLeft = BattleContext.BattleManager.Director.PlayerShip.Hull.Cargo;
		BattleContext.BattleManager.Director.PlayerShip.OnHealBegin();
		m_activeZone.SetActive(true);
		m_noSalvation.SetActive(false);

		BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.HealStationUse++;
		BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.TotalCargoBrought += Mathf.RoundToInt(m_healthLeft);
	}

	private void PerformHealPlayerState() {
		Vector3 playerPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
		if (Vector3.Distance(playerPosition, Position) > m_settings.HealingRadius + 0.05f) {
			ToWorkDoneState();
		}

		if (m_healthLeft <= 0) {
			ToWorkDoneState();
		}

		float hp = Time.fixedDeltaTime * m_settings.EnergyPerSecond;
		m_healthLeft -= hp;
		BattleContext.BattleManager.Director.PlayerShip.OnHeal(hp);
	}

	private void ToWorkDoneState() {
		m_state = HealthDroneStationState.WorkDone;

		foreach (HealthDrone drone in m_drones) {
			drone.ToMoveToBaseState();
		}

		BattleContext.BattleManager.Director.PlayerShip.OnHealEnd();
		m_activeZone.SetActive(false);
		m_sleepZone.SetActive(false);
		m_noSalvation.SetActive(false);

		m_animator.SetBool("Off", true);
	}

	private void PerformWorkDoneState() {
		
	}

	public HealthDroneStationState State {
		get {
			return m_state;
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 0;
		}
	}

	private HealthDrone CreateHealthDrone() {
		HealthDrone healthDrone = (Instantiate(m_healthDronePrefab)).GetComponent<HealthDrone>();
		healthDrone.transform.parent = BattleContext.BattleManager.AlliesController.gameObject.transform;
		healthDrone.Initiate();
		m_drones.Add(healthDrone);
		return healthDrone;
	}

}

public enum HealthDroneStationState {
	Sleep,
	NoSalvation,
	HealPlayer,
	WorkDone
}
