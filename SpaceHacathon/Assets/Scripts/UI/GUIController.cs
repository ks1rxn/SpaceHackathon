using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rotationJoystick;
	[SerializeField]
	private GameObject m_joystickHandle;
	[SerializeField]
	private Slider m_powerBar;
	[SerializeField]
	private Image m_health;
    [SerializeField]
	private Slider m_charge;
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

	public void SetRightJoystickAngle(float angle) {
//		float radius = 8;
//#if UNITY_EDITOR
//		radius = 16;
//#endif
//		float radius = 0.01f;
//		m_joystickHandle.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) * Screen.width / radius, Mathf.Sin(angle * Mathf.PI / 180) * Screen.width / radius, 0);
	}

	public void SetLeftJoysticValue(float value) {
		m_powerBar.value = value / 2 + 0.5f;
	}

	public void SetHealth(float value) {
		m_health.fillAmount = 1 - value;
	}

    public void SetCharge(float value) {
        m_charge.value = 1 - value;
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

	public Vector3 RotationJoystickCenter {
		get {
			return m_rotationJoystick.transform.position;
		}
	}

}
