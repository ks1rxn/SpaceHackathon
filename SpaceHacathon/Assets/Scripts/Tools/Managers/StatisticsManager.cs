using UnityEngine;

public class StatisticsManager : MonoBehaviour {
	private float m_fps;
	private int m_frames;

	private int m_fpsUnder30;
	private int m_fps3050;
	private int m_fpsAbove50;

	[SerializeField]
	private GoogleAnalyticsV4 m_analytics;

	private PlayerShipStatistics m_playerShipStatistics;

	public void Initiate() {
		m_playerShipStatistics = new PlayerShipStatistics();		
	}

	public void UpdateEntity() {
		int fps = Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
		m_fps += fps;
		if (fps > 50) {
			m_fpsAbove50++;
		} else if (fps < 30) {
			m_fpsUnder30++;
		} else {
			m_fps3050++;
		}
		m_frames++;
	}

	public void SendPlayerShipStatistics() {
		string category = "BattleScene-0.2.2:1-" + BattleContext.NextLevel;
		string eventName = "EndBattle";

		float perc30 = (float)m_fpsUnder30 / m_frames * 100;
		float perc3050 = (float)m_fps3050 / m_frames * 100;
		float perc50 = (float)m_fpsAbove50 / m_frames * 100;

		m_analytics.LogEvent("fps", SystemInfo.deviceModel, "avg", Mathf.Clamp(Mathf.RoundToInt(m_fps / m_frames), 0, 100));
		m_analytics.LogEvent("fps", SystemInfo.deviceModel, "under30", Mathf.RoundToInt(perc30));
		m_analytics.LogEvent("fps", SystemInfo.deviceModel, "3050", Mathf.RoundToInt(perc3050));
		m_analytics.LogEvent("fps", SystemInfo.deviceModel, "above50", Mathf.RoundToInt(perc50));

		m_analytics.LogEvent(category, eventName, "TimeAlive", (int)BattleContext.BattleManager.TimeManager.GameTime);

		m_analytics.LogEvent(category, eventName, "RamShipHit", m_playerShipStatistics.RamShipHit);
		m_analytics.LogEvent(category, eventName, "MineHit", m_playerShipStatistics.MineHit);
		m_analytics.LogEvent(category, eventName, "EnemyShipHit", m_playerShipStatistics.EnemyShipHit);

		m_analytics.LogEvent(category, eventName, "KillDroneCarrier", m_playerShipStatistics.KillDroneCarrier);
		m_analytics.LogEvent(category, eventName, "KillRamShip", m_playerShipStatistics.KillRamShip);
		m_analytics.LogEvent(category, eventName, "KillRocketShip", m_playerShipStatistics.KillRocketShip);
		m_analytics.LogEvent(category, eventName, "KillStunShip", m_playerShipStatistics.KillStunShip);

		m_analytics.LogEvent(category, eventName, "CarrierRocketHit", m_playerShipStatistics.CarrierRocketHit);
		m_analytics.LogEvent(category, eventName, "MissileHit", m_playerShipStatistics.MissileHit);
		m_analytics.LogEvent(category, eventName, "StunHit", m_playerShipStatistics.StunHit);
		m_analytics.LogEvent(category, eventName, "LaserHit", m_playerShipStatistics.LaserHit);
		m_analytics.LogEvent(category, eventName, "TimeInSlowingCloud", Mathf.RoundToInt(m_playerShipStatistics.TimeInSlowingCloud));

		m_analytics.LogEvent(category, eventName, "EnergyBarrelTake", m_playerShipStatistics.EnergyBarrelTake);
		m_analytics.LogEvent(category, eventName, "HealStationUse", m_playerShipStatistics.HealStationUse);
		m_analytics.LogEvent(category, eventName, "TotalCargoBrought", m_playerShipStatistics.TotalCargoBrought);
		m_analytics.LogEvent(category, eventName, "NoSalvation", m_playerShipStatistics.NoSalvation);
		m_analytics.LogEvent(category, eventName, "ChargeUsed", m_playerShipStatistics.ChargeUsed);

		m_analytics.LogEvent(category, eventName, "TimeOn1Battery", Mathf.RoundToInt(m_playerShipStatistics.TimeOn1Battery));
		m_analytics.LogEvent(category, eventName, "TimeOn2Battery", Mathf.RoundToInt(m_playerShipStatistics.TimeOn2Battery));
		m_analytics.LogEvent(category, eventName, "TimeOn3Battery", Mathf.RoundToInt(m_playerShipStatistics.TimeOn3Battery));
		m_analytics.LogEvent(category, eventName, "TimeOn4Battery", Mathf.RoundToInt(m_playerShipStatistics.TimeOn4Battery));
		m_analytics.LogEvent(category, eventName, "TimeOn5Battery", Mathf.RoundToInt(m_playerShipStatistics.TimeOn5Battery));
}

	public PlayerShipStatistics PlayerShipStatistics {
		get {
			return m_playerShipStatistics;
		}
	}

}

public class PlayerShipStatistics {
	public int MissileHit { get; set; }
	public int CarrierRocketHit { get; set; }
	public int StunHit { get; set; }
	public int LaserHit { get; set; }

	public int EnemyShipHit { get; set; }
	public int RamShipHit { get; set; }
	public int MineHit { get; set; }

	public int KillDroneCarrier { get; set; }
	public int KillRamShip { get; set; }
	public int KillStunShip { get; set; }
	public int KillRocketShip { get; set; }

	public int ChargeUsed { get; set; }

	public int EnergyBarrelTake { get; set; }
	public float TimeInSlowingCloud { get; set; }

	public int HealStationUse { get; set; }
	public int TotalCargoBrought { get; set; }
	public int NoSalvation { get; set; }

	public float TimeOn1Battery { get; set; }
	public float TimeOn2Battery { get; set; }
	public float TimeOn3Battery { get; set; }
	public float TimeOn4Battery { get; set; }
	public float TimeOn5Battery { get; set; }
}