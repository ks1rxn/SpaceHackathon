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

		m_analytics.LogScreen(new AppViewHitBuilder().SetScreenName("BattleScene").SetCustomDimension(1, SystemInfo.deviceModel));
		m_analytics.LogEvent("GameProcess2", "StartGame", "Director", -1);
		m_analytics.LogEvent(new EventHitBuilder().SetEventCategory("GP").SetEventAction("SG").SetEventLabel("D").SetEventValue(2)
			.SetCustomMetric(3, "test"));
	}

	public void OnPlayerDie() {
		
	}

	public void OnEnemyKill(IEnemyShip enemy) {
		
	}

}