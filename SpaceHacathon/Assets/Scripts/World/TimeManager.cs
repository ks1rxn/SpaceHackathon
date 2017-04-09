using UnityEngine;

public class TimeManager : MonoBehaviour {
	private TimeScaleMode m_timeScaleMode;
    private bool m_onPause;

	public float GameTime { get; set; }

	public void Initiate() {
		GameTime = 0;

		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		m_timeScaleMode = TimeScaleMode.Normal;
	}

	public void SetTimeScaleMode(TimeScaleMode mode) {
		m_timeScaleMode = mode;
	}

	public void Pause() {
		Time.timeScale = 0;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		m_onPause = true;
	}

	public void Unpause() {
		m_onPause = false;
		switch (m_timeScaleMode) {
			case TimeScaleMode.Normal:
				Time.timeScale = 1.0f;
				break;
			case TimeScaleMode.SuperSlow:
				Time.timeScale = 0.1f;
				break;
		}
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

    public void UpdateEntity() {
        UpdateTimeSpeed();

        GameTime += Time.deltaTime;
		BattleContext.BattleManager.GUIManager.PlayerGUIController.SetTime(GameTime);
    }

    private void UpdateTimeSpeed() {
	    if (m_onPause) {
		    return;
	    }
	    switch (m_timeScaleMode) {
			case TimeScaleMode.Normal:
				if (Time.timeScale < 1) {
					Time.timeScale += Time.deltaTime * 8;
				}
			    break;
			case TimeScaleMode.SuperSlow:
				if (Time.timeScale > 0.1f) {
					Time.timeScale -= Time.deltaTime * 4;
				} else if (Time.time < 0.08) {
					Time.timeScale += Time.deltaTime * 4;
				}
			    break;
	    }
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

}

public enum TimeScaleMode {
	SuperSlow,
	Normal
}