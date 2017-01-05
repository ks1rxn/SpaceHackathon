using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class World : MonoBehaviour {
	private TimeScaleMode m_timeScaleMode;
    private bool m_slowModeOn;
    private float m_points;

    private void Awake() {
        BattleContext.World = this;
        m_points = 0;
		SetTimeScaleMode(TimeScaleMode.Normal);

		Analytics.CustomEvent("gameStart", new Dictionary<string, object> {
			{ "device", SystemInfo.deviceModel },
			{ "platform", Application.platform },
			{ "install", Application.installMode },
			{ "version", Application.version }
		});
    }

	private void Start() {
		BattleContext.PlayerShip.Initiate();
	}

	public void SetTimeScaleMode(TimeScaleMode mode) {
		m_timeScaleMode = mode;
	}

    private void Update() {
        UpdateTimeSpeed();

        m_points += Time.deltaTime;
        BattleContext.GUIController.SetPoints(m_points);
    }

    private void UpdateTimeSpeed() {
	    switch (m_timeScaleMode) {
			case TimeScaleMode.Normal:
				if (Time.timeScale < 1) {
					Time.timeScale += Time.deltaTime * 8;
				}
			    break;
			case TimeScaleMode.SuperSlow:
				if (Time.timeScale > 0.1f) {
					Time.timeScale -= Time.deltaTime * 4;
				} 
			    break;
	    }
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

    public float Points {
        get {
            return m_points;
        }
    }
}

public enum TimeScaleMode {
	SuperSlow,
	Normal
}