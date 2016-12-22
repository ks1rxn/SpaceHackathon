using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rotationJoystick;
	[SerializeField]
	private GameObject m_joystickHandle;
	[SerializeField]
	private Image m_rotationDirection;

	
	[SerializeField]
	private Image m_health;
	[SerializeField]
	private Image[] m_chargeIndicator;
	[SerializeField]
	private Sprite m_chargeBlue;
	[SerializeField]
	private Sprite m_chargeOrange;

	[SerializeField]
	private Slider m_powerBar;
	[SerializeField]
	private Text m_fpsCounter;
    [SerializeField]
    private Text m_pointsCounter;

    [SerializeField]
    private Image m_deadPanel;
    [SerializeField]
    private Text m_deadLabel;

	private Queue<int> m_fps; 

	protected void Awake() {
		BattleContext.GUIController = this;
		m_fps = new Queue<int>();
	}

	protected void Update() {
		m_fps.Enqueue(Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale));
		if (m_fps.Count > 30) {
			m_fps.Dequeue();
		}
		float average = 0;
		int min = 1000;
		foreach (int fps in m_fps) {
			average += fps;
			if (fps < min) {
				min = fps;
			}
		}
		average /= m_fps.Count;
		m_fpsCounter.text = "FPS: " + Mathf.RoundToInt(average) + " - " + min;
	}

	public void SetLeftJoysticValue(float value) {
		m_powerBar.value = value / 2 + 0.5f;
	}

    public void SetPoints(float points) {
        m_pointsCounter.text = ((int)points).ToString();
    }

    public void SetDeadPanelOpacity(float opacity) {
        Color color = m_deadPanel.color;
        color.a = opacity;
        m_deadPanel.color = color;

        color = m_deadLabel.color;
        color.a = opacity;
        m_deadLabel.color = color;
    }

    public void SetDeadScore(float score) {
        m_deadLabel.text = "Score : " + (int) score;
    }

	// Health + Power //

	public void SetHealth(float value) {
		m_health.fillAmount = value;
	}

    public void SetCharge(int value) {
	    for (int i = 0; i != 5; i++) {
		    m_chargeIndicator[i].gameObject.SetActive(i < value);
	    }
    }

	// Rotation Joystick //

	public void SetRotationParams(float angleTo, float diff) {
		m_rotationDirection.transform.eulerAngles = new Vector3(0, 0, angleTo);
		m_rotationDirection.fillClockwise = diff < 0;
		m_rotationDirection.fillAmount = Mathf.Abs(diff) / 360f;
	}

	public void SetRightJoystickAngle(float angle) {
		//fucking hack: all questions to @Gigamesh
		m_joystickHandle.transform.eulerAngles = new Vector3(0, 0, -60 + angle);
	}

	public Vector3 RotationJoystickCenter {
		get {
			return m_rotationJoystick.transform.position;
		}
	}

}
