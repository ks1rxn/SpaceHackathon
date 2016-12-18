using UnityEngine;

public class World : MonoBehaviour {
	private TimeScaleMode m_timeScaleMode;
    private bool m_slowModeOn;
    private float m_points;

    private void Awake() {
        BattleContext.World = this;
        m_points = 0;
		SetTimeScaleMode(TimeScaleMode.Normal);
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
					Time.timeScale += Time.deltaTime * 8;
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
				if (Time.timeScale > 0.1f) {
					Time.timeScale -= Time.deltaTime * 2;
				} else if (Time.timeScale < 0.09f) {
					Time.timeScale += Time.deltaTime * 2;
				}
			    break;
			case TimeScaleMode.Stopped:
				if (Time.timeScale > 0f) {
					float newScale = Time.timeScale - Time.deltaTime * 8;
					Time.timeScale = newScale < 0 ? 0 : newScale;
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
	Stopped,
	SuperSlow,
	Slow,
	Normal
}