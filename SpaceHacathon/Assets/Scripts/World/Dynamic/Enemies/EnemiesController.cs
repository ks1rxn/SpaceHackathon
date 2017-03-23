using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : IController {
	private SettingsEnemiesController m_settings;

	[SerializeField]
	private GameObject m_blasterShipPrefab;
	[SerializeField]
	private GameObject m_droneCarrierPrefab;
	[SerializeField]
	private GameObject m_ramShipPrefab;
	[SerializeField]
	private GameObject m_spaceMinePrefab;
	[SerializeField]
	private GameObject m_rocketShipPrefab;

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
				CreateDroneCarrier();
			}
		}
		if (m_settings.SpawnRocketShip) {
			for (int i = 0; i != m_settings.RocketShipCount; i++) {
				CreateRocketShip();
			}
		}
		if (m_settings.SpawnSpaceMine) {
			for (int i = 0; i != m_settings.SpaceMineCount; i++) {
				CreateSpaceMine();
			}
		}
		
		m_ramShipCooldown = MathHelper.Random.Next(m_settings.RamShipCooldownDispertion * 2) - m_settings.RamShipCooldownDispertion + m_settings.RamShipCooldownValue;
		m_ramShipAlive = false;

		m_stunShipCooldown = MathHelper.Random.Next(m_settings.StunShipCooldownDispertion * 2) - m_settings.StunShipCooldownDispertion + m_settings.StunShipCooldownValue;
		m_stunShipAlive = false;
	}

	public override void FixedUpdateEntity() {
		if (m_settings.SpawnRamShip && !m_ramShipAlive) {
			if (m_ramShipCooldown > 0) {
				m_ramShipCooldown -= Time.fixedDeltaTime;
			} else {
				SpawnRamShip(MathHelper.GetPointAround(BattleContext.PlayerShip.Position, m_settings.RamShipSpawnMinDistance, m_settings.RamShipSpawnMaxDistance), 0);
				m_ramShipAlive = true;
			}
		}

		if (m_settings.SpawnStunShip && !m_stunShipAlive) {
			if (m_stunShipCooldown > 0) {
				m_stunShipCooldown -= Time.fixedDeltaTime;
			} else {
				SpawnStunShip(MathHelper.GetPointAround(BattleContext.PlayerShip.Position, m_settings.StunShipSpawnMinDistance, m_settings.StunShipSpanwMaxDistance), 0);
				m_stunShipAlive = true;
			}
		}

		for (int i = 0; i != m_ships.Count; i++) {
			if (m_ships[i].IsSpawned()) {
				m_ships[i].FixedUpdateEntity();
			} else {
				if (m_ships[i] is DroneCarrier) {
					Vector3 dcPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_settings.DroneCarrierSpawnAngle, m_settings.DroneCarrierSpawnMinDistance, m_settings.DroneCarrierSpawnMaxDistance);
					dcPosition.y = -0.75f;
					SpawnDroneCarrier(dcPosition, MathHelper.Random.Next(360));
				}
				if (m_ships[i] is RocketShip) {
					Vector3 rsPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_settings.RocketShipSpawnAngle, m_settings.RocketShipSpawnMinDistance, m_settings.RocketShipSpawnMaxDistance);
					SpawnRocketShip(rsPosition, MathHelper.Random.Next(360));
				}
				if (m_ships[i] is SpaceMine) {
					Vector3 spaceMinePosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_settings.SpaceMineSpawnAngle, m_settings.SpaceMineSpawnMinDistance, m_settings.SpaceMineSpawnMaxDistance);
					spaceMinePosition.y = -2.5f;
					SpawnSpaceMine(spaceMinePosition);
				}
			}
		}
	}

	public void OnRamShipDie() {
		m_ramShipCooldown = MathHelper.Random.Next(m_settings.RamShipCooldownDispertion * 2) - m_settings.RamShipCooldownDispertion + m_settings.RamShipCooldownValue;
		m_ramShipAlive = false;
	}

	public void OnStunShipDie() {
		m_stunShipCooldown = MathHelper.Random.Next(m_settings.StunShipCooldownDispertion * 2) - m_settings.StunShipCooldownDispertion + m_settings.StunShipCooldownValue;
		m_stunShipAlive = false;
	}

	public StunShip SpawnStunShip(Vector3 position, float rotation) {
		StunShip targetShip = null;
		foreach (StunShip ship in m_stunShips) {
			if (!ship.IsSpawned()) {
				targetShip = ship;
				break;
			}
		}
		if (targetShip == null) {
			targetShip = CreateStunShip();
		}
		targetShip.Spawn(position, rotation);
		return targetShip;
	}

	public DroneCarrier SpawnDroneCarrier(Vector3 position, float rotation) {
		DroneCarrier targetShip = null;
		foreach (DroneCarrier ship in m_droneCarriers) {
			if (!ship.IsSpawned()) {
				targetShip = ship;
				break;
			}
		}
		if (targetShip == null) {
			targetShip = CreateDroneCarrier();
		}
		targetShip.Spawn(position, rotation);
		return targetShip;
	}

	public RocketShip SpawnRocketShip(Vector3 position, float rotation) {
		RocketShip targetShip = null;
		foreach (RocketShip ship in m_rocketShips) {
			if (!ship.IsSpawned()) {
				targetShip = ship;
				break;
			}
		}
		if (targetShip == null) {
			targetShip = CreateRocketShip();
		}
		targetShip.Spawn(position, rotation);
		return targetShip;
	}

	public RamShip SpawnRamShip(Vector3 position, float rotation) {
		RamShip targetShip = null;
		foreach (RamShip ship in m_ramShips) {
			if (!ship.IsSpawned()) {
				targetShip = ship;
				break;
			}
		}
		if (targetShip == null) {
			targetShip = CreateRamShip();
		}
		targetShip.Spawn(position, rotation);
		return targetShip;
	}

	public SpaceMine SpawnSpaceMine(Vector3 position) {
		SpaceMine targetShip = null;
		foreach (SpaceMine ship in m_spaceMines) {
			if (!ship.IsSpawned()) {
				targetShip = ship;
				break;
			}
		}
		if (targetShip == null) {
			targetShip = CreateSpaceMine();
		}
		targetShip.Spawn(position, 0);
		return targetShip;
	}

	private StunShip CreateStunShip() {
		StunShip stunShip = (Instantiate(m_blasterShipPrefab)).GetComponent<StunShip>();
		stunShip.transform.parent = transform;
		stunShip.Initiate();
		m_ships.Add(stunShip);
		m_stunShips.Add(stunShip);
		return stunShip;
	}

	private DroneCarrier CreateDroneCarrier() {
		DroneCarrier droneCarrier = (Instantiate(m_droneCarrierPrefab)).GetComponent<DroneCarrier>();
		droneCarrier.transform.parent = transform;
		droneCarrier.Initiate();
		m_ships.Add(droneCarrier);
		m_droneCarriers.Add(droneCarrier);
		return droneCarrier;
	}

	private RocketShip CreateRocketShip() {
		RocketShip rocketShip = (Instantiate(m_rocketShipPrefab)).GetComponent<RocketShip>();
		rocketShip.transform.parent = transform;
		rocketShip.Initiate();
		m_ships.Add(rocketShip);
		m_rocketShips.Add(rocketShip);
		return rocketShip;
	}

	private RamShip CreateRamShip() {
		RamShip ramShip = (Instantiate(m_ramShipPrefab)).GetComponent<RamShip>();
		ramShip.transform.parent = transform;
		ramShip.Initiate();
		m_ships.Add(ramShip);
		m_ramShips.Add(ramShip);
		return ramShip;
	}

	private SpaceMine CreateSpaceMine() {
		SpaceMine spaceMine = (Instantiate(m_spaceMinePrefab)).GetComponent<SpaceMine>();
		spaceMine.transform.parent = transform;
		spaceMine.Initiate();
		m_ships.Add(spaceMine);
		m_spaceMines.Add(spaceMine);
		return spaceMine;
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
