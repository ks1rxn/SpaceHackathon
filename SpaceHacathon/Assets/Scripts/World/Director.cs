using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Director : MonoBehaviour {

	private void Start() {
		Analytics.CustomEvent("gameStart", new Dictionary<string, object> {
			{ "device", SystemInfo.deviceModel },
			{ "platform", Application.platform },
			{ "install", Application.installMode },
			{ "version", Application.version }
		});

		BattleContext.PlayerShip.Initiate();
		BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
	}

}