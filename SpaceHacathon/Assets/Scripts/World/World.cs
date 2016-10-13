using UnityEngine;

public class World : MonoBehaviour {
    private bool m_slowModeOn;
    private float m_points;

    private void Awake() {
        BattleContext.World = this;
        m_points = 0;
    }

    public void TurnSlowMode(bool on) {
        m_slowModeOn = on;
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
		if (m_slowModeOn) {
			if (Time.timeScale > 0.3f) {
				Time.timeScale -= Time.deltaTime * 5;
			}
		} else {
			if (Time.timeScale < 1) {
				Time.timeScale += Time.deltaTime * 5;
			}
		}
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

    public float Points {
        get {
            return m_points;
        }
    }
}
