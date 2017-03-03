﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {
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

	[SerializeField]
	private bool m_enableDC, m_enableRS, m_enableSM, m_enableRAM, m_enableSTUN;
	[SerializeField]
	private int m_DCCount, m_DCSpawnAngle, m_DCSpawnMinDistance, m_DCSpawnMaxDistance;
	[SerializeField]
	private int m_RSCount, m_RSSpawnAngle, m_RSSpawnMinDistance, m_RSSpawnMaxDistance;
	[SerializeField]
	private int m_SMCount, m_SMSpawnAngle, m_SMSpawnMinDistance, m_SMSpawnMaxDistance;
	[SerializeField]
	private int m_RAMCooldownValue, m_RAMCooldownDispertion, m_RAMSpawnMinDistance, m_RAMSpawnMaxDistance;
	[SerializeField]
	private int m_STUNCooldownValue, m_STUNCooldownDispertion, m_STUNSpawnMinDistance, m_STUNSpanwMaxDistance;

	public void Initiate() {
		m_ships = new List<IEnemyShip>();
		m_ramShips = new List<RamShip>();
		m_stunShips = new List<StunShip>();
		m_spaceMines = new List<SpaceMine>();
		m_droneCarriers = new List<DroneCarrier>();
		m_rocketShips = new List<RocketShip>();
		if (m_enableDC) {
			for (int i = 0; i != m_DCCount; i++) {
				CreateDroneCarrier();
			}
		}
		if (m_enableRS) {
			for (int i = 0; i != m_RSCount; i++) {
				CreateRocketShip();
			}
		}
		if (m_enableSM) {
			for (int i = 0; i != m_SMCount; i++) {
				CreateSpaceMine();
			}
		}
		
		m_ramShipCooldown = MathHelper.Random.Next(m_RAMCooldownDispertion * 2) - m_RAMCooldownDispertion + m_RAMCooldownValue;
		m_ramShipAlive = false;

		m_stunShipCooldown = MathHelper.Random.Next(m_STUNCooldownDispertion * 2) - m_STUNCooldownDispertion + m_STUNCooldownValue;
		m_stunShipAlive = false;
	}

	public void UpdateEntity() {
		if (m_enableRAM && !m_ramShipAlive) {
			if (m_ramShipCooldown > 0) {
				m_ramShipCooldown -= Time.fixedDeltaTime;
			} else {
				SpawnRamShip(MathHelper.GetPointAround(BattleContext.PlayerShip.Position, m_RAMSpawnMinDistance, m_RAMSpawnMaxDistance), 0);
				m_ramShipAlive = true;
			}
		}

		if (m_enableSTUN && !m_stunShipAlive) {
			if (m_stunShipCooldown > 0) {
				m_stunShipCooldown -= Time.fixedDeltaTime;
			} else {
				SpawnStunShip(MathHelper.GetPointAround(BattleContext.PlayerShip.Position, m_STUNSpawnMinDistance, m_STUNSpanwMaxDistance), 0);
				m_stunShipAlive = true;
			}
		}

		for (int i = 0; i != m_ships.Count; i++) {
			if (m_ships[i].IsAlive) {
				m_ships[i].UpdateShip();
			} else {
				if (m_ships[i] is DroneCarrier) {
					Vector3 dcPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_DCSpawnAngle, m_DCSpawnMinDistance, m_DCSpawnMaxDistance);
					dcPosition.y = -0.75f;
					SpawnDroneCarrier(dcPosition, MathHelper.Random.Next(360));
				}
				if (m_ships[i] is RocketShip) {
					Vector3 rsPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_RSSpawnAngle, m_RSSpawnMinDistance, m_RSSpawnMaxDistance);
					SpawnRocketShip(rsPosition, MathHelper.Random.Next(360));
				}
				if (m_ships[i] is SpaceMine) {
					Vector3 spaceMinePosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.SpeedVector, m_SMSpawnAngle, m_SMSpawnMinDistance, m_SMSpawnMaxDistance);
					spaceMinePosition.y = -2.5f;
					SpawnSpaceMine(spaceMinePosition);
				}
			}
		}
	}

	public void OnRamShipDie() {
		m_ramShipCooldown = MathHelper.Random.Next(m_RAMCooldownDispertion * 2) - m_RAMCooldownDispertion + m_RAMCooldownValue;
		m_ramShipAlive = false;
	}

	public void OnStunShipDie() {
		m_stunShipCooldown = MathHelper.Random.Next(m_STUNCooldownDispertion * 2) - m_STUNCooldownDispertion + m_STUNCooldownValue;
		m_stunShipAlive = false;
	}

	public StunShip SpawnStunShip(Vector3 position, float rotation) {
		StunShip targetShip = null;
		foreach (StunShip ship in m_stunShips) {
			if (!ship.IsAlive) {
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
			if (!ship.IsAlive) {
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
			if (!ship.IsAlive) {
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
			if (!ship.IsAlive) {
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
			if (!ship.IsAlive) {
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
