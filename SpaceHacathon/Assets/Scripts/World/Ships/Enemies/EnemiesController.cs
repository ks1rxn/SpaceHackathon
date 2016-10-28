using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemiesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketShipPrefab;
	[SerializeField]
	private GameObject m_blasterShipPrefab;

	private List<RocketShip> m_rocketShips;
	private List<BlasterShip> m_blasterShips;

	private Random m_random = new Random();

	private void Awake() {
		BattleContext.EnemiesController = this;

		m_rocketShips = new List<RocketShip>();
		m_blasterShips = new List<BlasterShip>();

		for (int i = 0; i != 20; i++) {
			CreateRocketShip();
		}
		for (int i = 0; i != 2; i++) {
			CreateBlasterShip();
		}
	}

	private void Start() {
		foreach (RocketShip ship in m_rocketShips) {
			Respawn(ship);
		}
		foreach (BlasterShip ship in m_blasterShips) {
			Respawn(ship);
		}
	}

	public void Respawn(RocketShip ship) {
		Vector3 playerPos = BattleContext.PlayerShip.transform.position;
		float angle = (float) m_random.NextDouble() * 360;
		float distance = m_random.Next(30) + 25;
		ship.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance));
	}

	public void Respawn(BlasterShip ship) {
		Vector3 playerPos = BattleContext.PlayerShip.transform.position;
		float angle = (float) m_random.NextDouble() * 360;
		float distance = m_random.Next(30) + 25;
		ship.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance));
	}

	private RocketShip CreateRocketShip() {
		RocketShip rocketShip = (Instantiate(m_rocketShipPrefab)).GetComponent<RocketShip>();
		rocketShip.transform.parent = transform;
		m_rocketShips.Add(rocketShip);
		return rocketShip;
	}

	private BlasterShip CreateBlasterShip() {
		BlasterShip blasterShip = (Instantiate(m_blasterShipPrefab)).GetComponent<BlasterShip>();
		blasterShip.transform.parent = transform;
		m_blasterShips.Add(blasterShip);
		return blasterShip;
	}

}
