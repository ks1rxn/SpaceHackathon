using System;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.Statistics {

	public class StatisticsManager : MonoBehaviour {
		private float _fps;
		private int _frames;

		private int _fpsUnder30;
		private int _fps3050;
		private int _fpsAbove50;

		private GoogleAnalyticsV4 _analytics;
		private PlayerShipStatistics _playerShipStatistics;

		[Inject]
		public void Construct(GoogleAnalyticsV4 analytics) {
			_analytics = analytics;
			_playerShipStatistics = new PlayerShipStatistics();
		}

		public void UpdateEntity() {
			int fps = Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
			if (fps > 65 || fps < 10) {
				return;
			}
			_fps += fps;
			if (fps > 50) {
				_fpsAbove50++;
			} else if (fps < 30) {
				_fpsUnder30++;
			} else {
				_fps3050++;
			}
			_frames++;
		}

		public void SendPlayerShipStatistics() {
			string statisticsVersion = "BattleScene-0.2.31:1";
			string fpsName = statisticsVersion + "-fps";
			string category = statisticsVersion + "-" + BattleContext.NextLevel;
			string eventName = "EndBattle";

			float perc30 = (float)_fpsUnder30 / _frames * 100;
			float perc3050 = (float)_fps3050 / _frames * 100;
			float perc50 = (float)_fpsAbove50 / _frames * 100;

			_analytics.LogEvent(fpsName, SystemInfo.deviceModel, "avg", Mathf.Clamp(Mathf.RoundToInt(_fps / _frames), 0, 100));
			_analytics.LogEvent(fpsName, SystemInfo.deviceModel, "under30", Mathf.RoundToInt(perc30));
			_analytics.LogEvent(fpsName, SystemInfo.deviceModel, "3050", Mathf.RoundToInt(perc3050));
			_analytics.LogEvent(fpsName, SystemInfo.deviceModel, "above50", Mathf.RoundToInt(perc50));
		
			_analytics.LogEvent(category, eventName, "TimeAlive", (int)BattleContext.BattleManager.TimeManager.GameTime);
		
			Type type = _playerShipStatistics.GetType();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo property in properties) {
				float value;
				if (float.TryParse(property.GetValue(_playerShipStatistics).ToString(), out value)) {
					_analytics.LogEvent(category, eventName, property.Name, Mathf.RoundToInt(value));
				}
			}
		}

		public PlayerShipStatistics PlayerShipStatistics => _playerShipStatistics;

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

}