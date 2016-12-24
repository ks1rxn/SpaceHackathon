using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketShipPrefab;
	[SerializeField]
	private GameObject m_blasterShipPrefab;
	[SerializeField]
	private GameObject m_rocketLauncherPrefab;
	[SerializeField]
	private GameObject m_ramShipPrefab;

	private List<IEnemyShip> m_ships;

	private void Awake() {
		BattleContext.EnemiesController = this;

		m_ships = new List<IEnemyShip>();
		for (int i = 0; i != 5; i++) {
//			CreateRocketLauncher();
		}
		for (int i = 0; i != 5; i++) {
//			CreateBlasterShip();
		}
	}

	private void Start() {
		foreach (IEnemyShip ship in m_ships) {
			Respawn(ship);
		}
		StartCoroutine(DelayedSpawn());
	}

	private IEnumerator DelayedSpawn() {
		yield return new WaitForSecondsRealtime(0.5f);
		SpawnRamShipAt(new Vector3(4, 0, 0));
		SpawnLauncherAt(new Vector3(-3, 0, 0));
	}

	private void FixedUpdate() {
		for (int i = 0; i != m_ships.Count; i++) {
			m_ships[i].UpdateShip();
		}
	}

	public void SpawnRamShipAt(Vector3 position) {
		IEnemyShip ship = CreateRamShip();
		ship.Spawn(position, 0);
	}

	public void SpawnLauncherAt(Vector3 position) {
		IEnemyShip ship = CreateRocketLauncher();
		ship.Spawn(position, 0);
	}

	public void Respawn(IEnemyShip ship) {
		Vector3 playerPos = BattleContext.PlayerShip.Position;
		float angle = (float) MathHelper.Random.NextDouble() * 360;
		float distance = MathHelper.Random.Next(35) + 5;
		ship.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, -0.4f, playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance), 0);
	}

	private RocketShip CreateRocketShip() {
		RocketShip rocketShip = (Instantiate(m_rocketShipPrefab)).GetComponent<RocketShip>();
		rocketShip.transform.parent = transform;
		m_ships.Add(rocketShip);
		return rocketShip;
	}

	private BlasterShip CreateBlasterShip() {
		BlasterShip blasterShip = (Instantiate(m_blasterShipPrefab)).GetComponent<BlasterShip>();
		blasterShip.transform.parent = transform;
		m_ships.Add(blasterShip);
		return blasterShip;
	}

	private RocketLauncher CreateRocketLauncher() {
		RocketLauncher rocketLauncher = (Instantiate(m_rocketLauncherPrefab)).GetComponent<RocketLauncher>();
		rocketLauncher.transform.parent = transform;
		m_ships.Add(rocketLauncher);
		return rocketLauncher;
	}

	private RamShip CreateRamShip() {
		RamShip ramShip = (Instantiate(m_ramShipPrefab)).GetComponent<RamShip>();
		ramShip.transform.parent = transform;
		m_ships.Add(ramShip);
		return ramShip;
	}

	public List<IEnemyShip> Ships {
		get {
			return m_ships;
		}
	}

}
