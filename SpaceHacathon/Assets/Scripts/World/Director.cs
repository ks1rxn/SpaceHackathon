using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Director : MonoBehaviour {
	[SerializeField]
	private GoogleAnalyticsV4 m_analytics;

	private void Start() {
//		Analytics.CustomEvent("gameStart", new Dictionary<string, object> {
//			{ "device", SystemInfo.deviceModel },
//			{ "platform", Application.platform },
//			{ "install", Application.installMode },
//			{ "version", Application.version }
//		});

		BattleContext.PlayerShip.Initiate();
		BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);

		m_analytics.StartSession();
		m_analytics.LogEvent("GameProcess", "StartGame", "Director", 1);
	}

	public void OnPlayerDie() {
		
	}

	public void OnEnemyKill(IEnemyShip enemy) {
		
	}

}