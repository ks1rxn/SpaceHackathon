using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketShipPrefab;
	[SerializeField]
	private GameObject m_blasterShipPrefab;

	private List<IEnemyShip> m_ships;

	private void Awake() {
		BattleContext.EnemiesController = this;

		m_ships = new List<IEnemyShip>();

		for (int i = 0; i != 20; i++) {
			CreateRocketShip();
		}
		for (int i = 0; i != 2; i++) {
			CreateBlasterShip();
		}
	}

	private void Start() {
		foreach (IEnemyShip ship in m_ships) {
			Respawn(ship);
		}
	}

	public void Respawn(IEnemyShip ship) {
		Vector3 playerPos = BattleContext.PlayerShip.transform.position;
		float angle = (float) MathHelper.Random.NextDouble() * 360;
		float distance = MathHelper.Random.Next(30) + 25;
		ship.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance), 0);
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

	public List<IEnemyShip> Ships {
		get {
			return m_ships;
		}
	}

}
