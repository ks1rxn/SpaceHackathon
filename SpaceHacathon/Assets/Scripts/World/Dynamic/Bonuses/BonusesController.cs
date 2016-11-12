using System.Collections.Generic;
using UnityEngine;

public class BonusesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_chargeFuelPrefab;

	private List<ChargeFuel> m_chargeFuels;

	private void Awake() {
		BattleContext.BonusesController = this;

		m_chargeFuels = new List<ChargeFuel>();

		for (int i = 0; i != 20; i++) {
			CreateChargeFuel();
		}
	}

	private void Start() {
		foreach (ChargeFuel fuel in m_chargeFuels) {
			Respawn(fuel);
		}
	}

	public void Respawn(ChargeFuel fuel) {
		Vector3 playerPos = BattleContext.PlayerShip.transform.position;
		float angle = (float) MathHelper.Random.NextDouble() * 360;
		float distance = MathHelper.Random.Next(30) + 25;
		fuel.Spawn(new Vector3(playerPos.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0 , playerPos.z + Mathf.Sin(angle * Mathf.PI / 180) * distance));
	}

	private ChargeFuel CreateChargeFuel() {
		ChargeFuel chargeFuel = (Instantiate(m_chargeFuelPrefab)).GetComponent<ChargeFuel>();
		chargeFuel.transform.parent = transform;
		m_chargeFuels.Add(chargeFuel);
		return chargeFuel;
	}

	public List<ChargeFuel> ChargeFuels {
		get {
			return m_chargeFuels;
		}
	}

}
