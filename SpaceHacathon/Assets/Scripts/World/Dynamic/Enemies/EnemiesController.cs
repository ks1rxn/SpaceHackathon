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

	private void Awake() {
		BattleContext.EnemiesController = this;

		m_ships = new List<IEnemyShip>();
		m_ramShips = new List<RamShip>();
		m_stunShips = new List<StunShip>();
		m_rocketLaunchers = new List<RocketLauncher>();
		for (int i = 0; i != 5; i++) {
			CreateRocketLauncher();
		}
		for (int i = 0; i != 5; i++) {
			CreateStunShip();
		}
	}

	private void Start() {
		StartCoroutine(DelayedSpawn());
	}

	private IEnumerator DelayedSpawn() {
		yield return new WaitForSecondsRealtime(0.5f);
		SpawnRamShip(new Vector3(4, 0, 0), 90);
		SpawnRocketLauncher(new Vector3(-3, 0, 0), 0);
	}

	private void FixedUpdate() {
		for (int i = 0; i != m_ships.Count; i++) {
			if (m_ships[i].IsAlive) {
				m_ships[i].UpdateShip();
			}
		}
	}

//	public void Respawn(IEnemyShip ship) {
//		Vector3 playerPos = BattleContext.PlayerShip.Position;
//		float angle = (float) MathHelper.Random.NextDouble() * 360;
//		float distance = MathHelper.Random.Next(35) + 5;
//		ship.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, -0.4f, playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance), 0);
//	}
	
	public StunShip SpawnStunShip(Vector3 position, float rotation) {
		StunShip targetShip = null;
		foreach (StunShip ship in m_stunShips) {
			if (!ship) {
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
			if (!ship) {
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
			if (!ship) {
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
