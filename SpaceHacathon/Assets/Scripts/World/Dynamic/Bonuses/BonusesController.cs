using System.Collections.Generic;
using UnityEngine;

public class BonusesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_chargeFuelPrefab;

	private List<ChargeFuel> m_chargeFuels;

	public void Initiate() {
		m_chargeFuels = new List<ChargeFuel>();

		for (int i = 0; i != 20; i++) {
			CreateChargeFuel();
		}
	}

	public void UpdateEntity() {
		Vector3 playerPosition = BattleContext.PlayerShip.Position;
		for (int i = 0; i != m_chargeFuels.Count; i++) {
			if (!m_chargeFuels[i].IsAlive) {
				Respawn(m_chargeFuels[i]);
			} else {
				if (Vector3.Distance(m_chargeFuels[i].transform.position, playerPosition) < 50) {
					m_chargeFuels[i].UpdateState();
				} else if (Vector3.Distance(m_chargeFuels[i].transform.position, playerPosition) > 80) {
					m_chargeFuels[i].IsAlive = false;
				}
			}
		}
	}

	public void Respawn(ChargeFuel fuel) {
		Vector3 playerPos = BattleContext.PlayerShip.Position;
		float angle = (float) MathHelper.Random.NextDouble() * 360;
		float distance = MathHelper.Random.Next(30) + 25;
		fuel.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance));
	}

	private ChargeFuel CreateChargeFuel() {
		ChargeFuel chargeFuel = (Instantiate(m_chargeFuelPrefab)).GetComponent<ChargeFuel>();
		chargeFuel.transform.parent = transform;
		chargeFuel.Initiate();
		m_chargeFuels.Add(chargeFuel);
		return chargeFuel;
	}

	public List<ChargeFuel> ChargeFuels {
		get {
			return m_chargeFuels;
		}
	}

}
