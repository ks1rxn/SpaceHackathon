using UnityEngine;

public class StatisticsManager : MonoBehaviour {
	private float m_fps;
	private int m_frames;

	[SerializeField]
	private GoogleAnalyticsV4 m_analytics;

	private PlayerShipStatistics m_playerShipStatistics;

	public void Initiate() {
		m_playerShipStatistics = new PlayerShipStatistics();		
	}

	public void UpdateEntity() {
		m_fps += Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
		m_frames++;
	}

	public void SendPlayerShipStatistics() {
		m_analytics.LogEvent("BattleScene", "EndBattle", "TimeAlive", (int)BattleContext.TimeManager.GameTime);
		m_analytics.LogEvent("BattleScene", "EndBattle", "RamShipHit", m_playerShipStatistics.RamShipHit);
		m_analytics.LogEvent("BattleScene", "EndBattle", "MineHit", m_playerShipStatistics.MineHit);
		m_analytics.LogEvent("BattleScene", "EndBattle", "EnemyShipHit", m_playerShipStatistics.EnemyShipHit);
		m_analytics.LogEvent("BattleScene", "EndBattle", "ChargeUsed", m_playerShipStatistics.ChargeUsed);
		m_analytics.LogEvent("BattleScene", "EndBattle", "RocketHit", m_playerShipStatistics.RocketHit);
		m_analytics.LogEvent("BattleScene", "EndBattle", "StunHit", m_playerShipStatistics.StunHit);
		m_analytics.LogEvent("BattleScene", "EndBattle", "LaserHit", m_playerShipStatistics.LaserHit);
	}

	public PlayerShipStatistics PlayerShipStatistics {
		get {
			return m_playerShipStatistics;
		}
	}

}

public class PlayerShipStatistics {
	public int RocketHit { get; set; }
	public int StunHit { get; set; }
	public int LaserHit { get; set; }
	public int EnemyShipHit { get; set; }
	public int RamShipHit { get; set; }
	public int MineHit { get; set; }
	public int ChargeUsed { get; set; }
}