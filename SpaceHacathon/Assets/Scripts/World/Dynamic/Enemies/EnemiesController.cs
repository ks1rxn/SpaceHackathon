using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_blasterShipPrefab;
	[SerializeField]
	private GameObject m_rocketLauncherPrefab;
	[SerializeField]
	private GameObject m_ramShipPrefab;

	private List<IEnemyShip> m_ships;
	private List<RamShip> m_ramShips; 
	private List<StunShip> m_stunShips; 
	private List<RocketLauncher> m_rocketLaunchers;

	private float m_ramShipCooldown;
	private bool m_ramShipAlive;

	private float m_stunShipCooldown;
	private bool m_stunShipAlive;

	private void Awake() {
		BattleContext.EnemiesController = this;

		m_ships = new List<IEnemyShip>();
		m_ramShips = new List<RamShip>();
		m_stunShips = new List<StunShip>();
		m_rocketLaunchers = new List<RocketLauncher>();
		for (int i = 0; i != 4; i++) {
			CreateRocketLauncher();
		}
		for (int i = 0; i != 1; i++) {
			CreateStunShip();
		}
		for (int i = 0; i != 1; i++) {
			CreateRamShip();
		}

		m_ramShipCooldown = MathHelper.Random.Next(20);
		m_ramShipAlive = false;

		m_stunShipCooldown = MathHelper.Random.Next(5);
		m_stunShipAlive = false;
	}

	private void FixedUpdate() {
		if (!m_ramShipAlive) {
			if (m_ramShipCooldown > 0) {
				m_ramShipCooldown -= Time.fixedDeltaTime;
			} else {
				SpawnRamShip(MathHelper.GetPointAround(BattleContext.PlayerShip.Position, 30, 40), 0);
				m_ramShipAlive = true;
			}
		}

		if (!m_stunShipAlive) {
			if (m_stunShipCooldown > 0) {
				m_stunShipCooldown -= Time.fixedDeltaTime;
			} else {
				SpawnStunShip(MathHelper.GetPointAround(BattleContext.PlayerShip.Position, 30, 40), 0);
				m_stunShipAlive = true;
			}
		}

		for (int i = 0; i != m_ships.Count; i++) {
			if (m_ships[i].IsAlive) {
				m_ships[i].UpdateShip();
			} else {
				if (m_ships[i] is RocketLauncher) {
					Vector3 rocketLauncherPosition = MathHelper.GetPointAround(BattleContext.PlayerShip.Position, BattleContext.PlayerShip.LookVector, 90, 20, 30);
					rocketLauncherPosition.y = -0.75f;
					SpawnRocketLauncher(rocketLauncherPosition, MathHelper.Random.Next(360));
				}
			}
		}
	}

	public void OnRamShipDie() {
		m_ramShipCooldown = MathHelper.Random.Next(15) + 15;
		m_ramShipAlive = false;
	}

	public void OnStunShipDie() {
		m_stunShipCooldown = MathHelper.Random.Next(10) + 5;
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

	public RocketLauncher SpawnRocketLauncher(Vector3 position, float rotation) {
		RocketLauncher targetShip = null;
		foreach (RocketLauncher ship in m_rocketLaunchers) {
			if (!ship.IsAlive) {
				targetShip = ship;
				break;
			}
		}
		if (targetShip == null) {
			targetShip = CreateRocketLauncher();
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

	private StunShip CreateStunShip() {
		StunShip stunShip = (Instantiate(m_blasterShipPrefab)).GetComponent<StunShip>();
		stunShip.transform.parent = transform;
		stunShip.Initiate();
		m_ships.Add(stunShip);
		m_stunShips.Add(stunShip);
		return stunShip;
	}

	private RocketLauncher CreateRocketLauncher() {
		RocketLauncher rocketLauncher = (Instantiate(m_rocketLauncherPrefab)).GetComponent<RocketLauncher>();
		rocketLauncher.transform.parent = transform;
		rocketLauncher.Initiate();
		m_ships.Add(rocketLauncher);
		m_rocketLaunchers.Add(rocketLauncher);
		return rocketLauncher;
	}

	private RamShip CreateRamShip() {
		RamShip ramShip = (Instantiate(m_ramShipPrefab)).GetComponent<RamShip>();
		ramShip.transform.parent = transform;
		ramShip.Initiate();
		m_ships.Add(ramShip);
		m_ramShips.Add(ramShip);
		return ramShip;
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

	public List<RocketLauncher> RocketLaunchers {
		get {
			return m_rocketLaunchers;
		}
	}

}
