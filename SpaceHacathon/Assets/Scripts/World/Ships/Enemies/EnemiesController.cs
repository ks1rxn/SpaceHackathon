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

	protected void Awake() {
		BattleContext.EnemiesController = this;

		m_rocketShips = new List<RocketShip>();
		m_blasterShips = new List<BlasterShip>();
	}

	protected void Update() {
		if (m_rocketShips.Count < 20) {
			Vector3 playerPos = BattleContext.PlayerShip.transform.position;
			float angle = (float) m_random.NextDouble() * 360;
			float distance = m_random.Next(30) + 25;
			SpawnRocketShip(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance));
		}
		if (m_blasterShips.Count < 3) {
			Vector3 playerPos = BattleContext.PlayerShip.transform.position;
			float angle = (float) m_random.NextDouble() * 360;
			float distance = m_random.Next(30) + 25;
			SpawnBlasterShip(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance));
		}
	}

	private void SpawnRocketShip(Vector3 position) {
		RocketShip rocketShip = ((GameObject)Instantiate(m_rocketShipPrefab)).GetComponent<RocketShip>();
		rocketShip.transform.parent = transform;
		rocketShip.Spawn(position, transform);
		m_rocketShips.Add(rocketShip);
	}

	private void SpawnBlasterShip(Vector3 position) {
		BlasterShip blasterShip = ((GameObject)Instantiate(m_blasterShipPrefab)).GetComponent<BlasterShip>();
		blasterShip.transform.parent = transform;
		blasterShip.Spawn(position, transform);
		m_blasterShips.Add(blasterShip);
	}

	public void ShipDied(RocketShip ship) {
		m_rocketShips.Remove(ship);
	}

	public void ShipDied(BlasterShip ship) {
		m_blasterShips.Remove(ship);
	}

}
