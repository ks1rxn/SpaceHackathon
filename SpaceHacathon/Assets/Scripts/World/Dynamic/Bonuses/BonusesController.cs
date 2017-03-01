using System.Collections.Generic;
using UnityEngine;

public class BonusesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_chargeFuelPrefab;
	[SerializeField]
	private GameObject m_timeBonusPrefab;

	private List<ChargeFuel> m_chargeFuels;
	private List<TimeBonus> m_timeBonuses;

	[SerializeField]
	private bool m_enableFuel;
	[SerializeField]
	private bool m_enableTime;
	[SerializeField]
	private int m_fuelCount;
	[SerializeField]
	private float m_fuelSpawnMinDist, m_fuelSpawnMaxDist, m_fuelSpawnAngle;
	[SerializeField]
	private int m_timeCount;
	[SerializeField]
	private float m_timeSpawnMinDist, m_timeSpawnMaxDist, m_timeSpawnAngle;

	public void Initiate() {
		m_chargeFuels = new List<ChargeFuel>();
		m_timeBonuses = new List<TimeBonus>();

		if (m_enableFuel) {
			for (int i = 0; i != m_fuelCount; i++) {
				CreateChargeFuel();
			}
		}
		if (m_enableTime) {
			for (int i = 0; i != m_timeCount; i++) {
				CreateTimeBonus();
			}
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
		for (int i = 0; i != m_timeBonuses.Count; i++) {
			if (!m_timeBonuses[i].IsAlive) {
				Respawn(m_timeBonuses[i]);
			} else {
				if (Vector3.Distance(m_timeBonuses[i].transform.position, playerPosition) < 50) {
					m_timeBonuses[i].UpdateState();
				} else if (Vector3.Distance(m_timeBonuses[i].transform.position, playerPosition) > 80) {
					m_timeBonuses[i].IsAlive = false;
				}
			}
		}
	}

	private void Respawn(ChargeFuel fuel) {
		Vector3 playerPos = BattleContext.PlayerShip.Position;
		fuel.Spawn(MathHelper.GetPointAround(playerPos, BattleContext.PlayerShip.SpeedValue, m_fuelSpawnAngle, m_fuelSpawnMinDist, m_fuelSpawnMaxDist));
	}

	private void Respawn(TimeBonus fuel) {
		Vector3 playerPos = BattleContext.PlayerShip.Position;
		fuel.Spawn(MathHelper.GetPointAround(playerPos, BattleContext.PlayerShip.SpeedValue, m_timeSpawnAngle, m_timeSpawnMinDist, m_timeSpawnMaxDist));
	}

	private ChargeFuel CreateChargeFuel() {
		ChargeFuel chargeFuel = (Instantiate(m_chargeFuelPrefab)).GetComponent<ChargeFuel>();
		chargeFuel.transform.parent = transform;
		chargeFuel.Initiate();
		m_chargeFuels.Add(chargeFuel);
		return chargeFuel;
	}

	private TimeBonus CreateTimeBonus() {
		TimeBonus timeBonus = (Instantiate(m_timeBonusPrefab)).GetComponent<TimeBonus>();
		timeBonus.transform.parent = transform;
		timeBonus.Initiate();
		m_timeBonuses.Add(timeBonus);
		return timeBonus;
	}

	public List<ChargeFuel> ChargeFuels {
		get {
			return m_chargeFuels;
		}
	}

}
