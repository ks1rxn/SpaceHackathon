using UnityEngine;

public class World : MonoBehaviour {
	private TimeScaleMode m_timeScaleMode;
    private bool m_slowModeOn;
    private float m_points;

    private void Awake() {
        BattleContext.World = this;
        m_points = 0;
    }

	public void SetTimeScaleMode(TimeScaleMode mode) {
		m_timeScaleMode = mode;
	}

    public void AddPoints(float points) {
        m_points += points;
    }

    private void Update() {
        UpdateTimeSpeed();

        m_points += 2 * Time.deltaTime;
        BattleContext.GUIController.SetPoints(m_points);
    }

    private void UpdateTimeSpeed() {
	    switch (m_timeScaleMode) {
			case TimeScaleMode.Normal:
				if (Time.timeScale < 1) {
					Time.timeScale += Time.deltaTime * 4;
				}
			    break;
			case TimeScaleMode.Slow:
				if (Time.timeScale < 0.48f) {
					Time.timeScale += Time.deltaTime * 3;
				} else if (Time.timeScale > 0.52f) {
					Time.timeScale -= Time.deltaTime * 2;
				}
			    break;
			case TimeScaleMode.SuperSlow:
				if (Time.timeScale > 0.2f) {
					Time.timeScale -= Time.deltaTime * 2;
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
	Slow,
	Normal
}