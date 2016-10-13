﻿using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	[SerializeField]
	private GameObject m_button;
	[SerializeField]
	private GameObject m_smallButton;
	[SerializeField]
	private Slider m_powerBar;
	[SerializeField]
	private Slider m_health;
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

	protected void Awake() {
		BattleContext.GUIController = this;
	}

	protected void Update() {
		m_fpsCounter.text = "FPS: " + Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
	}

	public void SetRightJoystickAngle(float angle) {
		float radius = 8;
#if UNITY_EDITOR
		radius = 16;
#endif
		m_smallButton.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) * Screen.width / radius, Mathf.Sin(angle * Mathf.PI / 180) * Screen.width / radius, 0);
	}

	public void SetLeftJoysticValue(float value) {
		m_powerBar.value = value / 2 + 0.5f;
	}

	public void SetHealth(float value) {
		m_health.value = 1 - value;
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

	public GameObject Button {
		get {
			return m_button;
		}
	}
}
