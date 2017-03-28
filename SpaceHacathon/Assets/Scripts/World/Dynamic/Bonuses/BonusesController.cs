using System.Collections.Generic;
using UnityEngine;

public class BonusesController : IController {
	private SettingsBonusesController m_settings;

	[SerializeField]
	private GameObject m_chargeFuelPrefab;
	[SerializeField]
	private GameObject m_timeBonusPrefab;

	private List<ChargeFuel> m_chargeFuels;
//	private List<TimeBonus> m_timeBonuses;

	private TimeBonus m_timeBonus;

	public override void Initiate() {
		m_settings = BattleContext.Settings.BonusesController;

		m_chargeFuels = new List<ChargeFuel>();
//		m_timeBonuses = new List<TimeBonus>();

		if (m_settings.EnableFuel) {
			for (int i = 0; i != m_settings.FuelCount; i++) {
				CreateChargeFuel();
			}
		}
		CreateTimeBonus();
//		if (m_settings.EnableTime) {
//			for (int i = 0; i != m_settings.TimeCount; i++) {
//				CreateTimeBonus();
//			}
//		}
	}

	public override void FixedUpdateEntity() {
		for (int i = 0; i != m_chargeFuels.Count; i++) {
			if (!m_chargeFuels[i].IsSpawned()) {
				Respawn(m_chargeFuels[i]);
			} else {
				m_chargeFuels[i].FixedUpdateEntity();
			}
		}
		if (!m_timeBonus.IsSpawned()) {
			RespawnTimeBonus(m_timeBonus);
		}
//		for (int i = 0; i != m_timeBonuses.Count; i++) {
//			if (!m_timeBonuses[i].IsSpawned()) {
//				Respawn(m_timeBonuses[i]);
//			} else {
//				m_timeBonuses[i].FixedUpdateEntity();
//			}
//		}
	}

	private void Respawn(ChargeFuel fuel) {
		Vector3 playerPos = BattleContext.PlayerShip.Position;
		fuel.Spawn(MathHelper.GetPointAround(playerPos, BattleContext.PlayerShip.SpeedVector, m_settings.FuelSpawnAngle, m_settings.FuelSpawnMinDist, m_settings.FuelSpawnMaxDist), 0);
	}

//	private void Respawn(TimeBonus fuel) {
//		Vector3 playerPos = BattleContext.PlayerShip.Position;
//		fuel.Spawn(MathHelper.GetPointAround(playerPos, BattleContext.PlayerShip.SpeedVector, m_settings.TimeSpawnAngle, m_settings.TimeSpawnMinDist, m_settings.TimeSpawnMaxDist), 0);
//	}

	public void RespawnTimeBonus(TimeBonus bonus) {
		Vector3 playerPos = BattleContext.PlayerShip.Position;
		bonus.Spawn(MathHelper.GetPointAround(playerPos, m_settings.TimeSpawnMinDist, m_settings.TimeSpawnMaxDist), 0);
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
		m_timeBonus = timeBonus;
		return m_timeBonus;
	}

	public List<ChargeFuel> ChargeFuels {
		get {
			return m_chargeFuels;
		}
	}

	public TimeBonus TimeBonus {
		get {
			return m_timeBonus;
		}
	}

}
