using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : IController {
	private SettingsEnemiesController m_settings;

	private List<IEnemyShip> m_ships;
	private List<RamShip> m_ramShips; 
	private List<StunShip> m_stunShips; 
	private List<SpaceMine> m_spaceMines; 
	private List<RocketShip> m_rocketShips; 
	private List<DroneCarrier> m_droneCarriers;

	private float m_ramShipCooldown;
	private bool m_ramShipAlive;

	private float m_stunShipCooldown;
	private bool m_stunShipAlive;

	public override void Initiate() {
		m_settings = BattleContext.Settings.EnemiesController;

		m_ships = new List<IEnemyShip>();
		m_ramShips = new List<RamShip>();
		m_stunShips = new List<StunShip>();
		m_spaceMines = new List<SpaceMine>();
		m_droneCarriers = new List<DroneCarrier>();
		m_rocketShips = new List<RocketShip>();
		if (m_settings.SpawnDroneCarrier) {
			for (int i = 0; i != m_settings.DroneCarrierCount; i++) {
				EntitiesHelper.CreateEntity(AvailablePrefabs.DroneCarrier, gameObject, m_ships, m_droneCarriers);
			}
		}
		if (m_settings.SpawnRocketShip) {
			for (int i = 0; i != m_settings.RocketShipCount; i++) {
				EntitiesHelper.CreateEntity(AvailablePrefabs.RocketShip, gameObject, m_ships, m_rocketShips);
			}
		}
		if (m_settings.SpawnSpaceMine) {
			for (int i = 0; i != m_settings.SpaceMineCount; i++) {
				EntitiesHelper.CreateEntity(AvailablePrefabs.SpaceMine, gameObject, m_ships, m_spaceMines);
			}
		}

		m_ramShipCooldown = MathHelper.ValueWithDispertion(m_settings.RamShipCooldownValue, m_settings.RamShipCooldownDispertion);
		m_ramShipAlive = false;

		m_stunShipCooldown = MathHelper.ValueWithDispertion(m_settings.StunShipCooldownValue, m_settings.StunShipCooldownDispertion);
		m_stunShipAlive = false;
	}

	public override void FixedUpdateEntity() {
		if (m_settings.SpawnRamShip && !m_ramShipAlive) {
			if (m_ramShipCooldown > 0) {
				m_ramShipCooldown -= Time.fixedDeltaTime;
			} else {
				Vector3 ramPosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, m_settings.RamShipSpawnMinDistance, m_settings.RamShipSpawnMaxDistance);
				EntitiesHelper.SpawnEntity(AvailablePrefabs.RamShip, gameObject, m_ramShips, m_ships, ramPosition, 0);
				m_ramShipAlive = true;
			}
		}

		if (m_settings.SpawnStunShip && !m_stunShipAlive) {
			if (m_stunShipCooldown > 0) {
				m_stunShipCooldown -= Time.fixedDeltaTime;
			} else {
				Vector3 stunPosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, m_settings.StunShipSpawnMinDistance, m_settings.StunShipSpanwMaxDistance);
				EntitiesHelper.SpawnEntity(AvailablePrefabs.StunShip, gameObject, m_stunShips, m_ships, stunPosition, 0);
				m_stunShipAlive = true;
			}
		}

		for (int i = 0; i != m_ships.Count; i++) {
			if (m_ships[i].IsSpawned()) {
				m_ships[i].FixedUpdateEntity();
			} else {
				if (m_ships[i] is DroneCarrier) {
					Vector3 dcPosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.DroneCarrierSpawnAngle, m_settings.DroneCarrierSpawnMinDistance, m_settings.DroneCarrierSpawnMaxDistance);
					dcPosition.y = -0.75f;
					EntitiesHelper.SpawnEntity(AvailablePrefabs.DroneCarrier, gameObject, m_droneCarriers, m_ships, dcPosition, MathHelper.Random.Next(360));
				}
				if (m_ships[i] is RocketShip) {
					Vector3 rsPosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.RocketShipSpawnAngle, m_settings.RocketShipSpawnMinDistance, m_settings.RocketShipSpawnMaxDistance);
					EntitiesHelper.SpawnEntity(AvailablePrefabs.RocketShip, gameObject, m_rocketShips, m_ships, rsPosition, MathHelper.Random.Next(360));
				}
				if (m_ships[i] is SpaceMine) {
					Vector3 spaceMinePosition = MathHelper.GetPointAround(BattleContext.BattleManager.Director.PlayerShip.Position, BattleContext.BattleManager.Director.PlayerShip.SpeedVector, m_settings.SpaceMineSpawnAngle, m_settings.SpaceMineSpawnMinDistance, m_settings.SpaceMineSpawnMaxDistance);
					EntitiesHelper.SpawnEntity(AvailablePrefabs.SpaceMine, gameObject, m_spaceMines, m_ships, spaceMinePosition, 0);
				}
			}
		}
	}

	public void OnRamShipDie() {
		m_ramShipCooldown = MathHelper.ValueWithDispertion(m_settings.RamShipCooldownValue, m_settings.RamShipCooldownDispertion);
		m_ramShipAlive = false;
	}

	public void OnStunShipDie() {
		m_stunShipCooldown = MathHelper.ValueWithDispertion(m_settings.StunShipCooldownValue, m_settings.StunShipCooldownDispertion);
		m_stunShipAlive = false;
	}

	public List<IEnemyShip> Ships {
		get {
			return m_ships;
		}
	}

	public List<RamShip> RamShips {
		get {
			return m_ramShips;
		}
	}

	public List<StunShip> StunShips {
		get {
			return m_stunShips;
		}
	}

	public List<DroneCarrier> DroneCarriers {
		get {
			return m_droneCarriers;
		}
	}

	public List<SpaceMine> SpaceMines {
		get {
			return m_spaceMines;
		}
	}

}
