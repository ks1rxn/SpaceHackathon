using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIController : MonoBehaviour {
	[SerializeField]
	private ScreenInput m_screenInput;

	[SerializeField]
	private EnergyIndicator m_energyIndicator;
	[SerializeField]
	private CargoIndicator m_cargoIndicator;

	[SerializeField]
	private GameObject m_rotationJoystick;
	[SerializeField]
	private GameObject m_joystickHandle;
	[SerializeField]
	private Image m_rotationDirection;

	
	[SerializeField]
	private GameObject m_chargeButton;

	[SerializeField]
	private Image m_powerIndicator;

	[SerializeField]
	private Text m_fpsCounter;
    [SerializeField]
    private Text m_time;

	private Queue<int> m_fps; 

	protected void Awake() {
		m_fps = new Queue<int>();
	}

	protected void Update() {
		m_screenInput.ListerInput();

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

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	// Time //

	public void SetTime(float floatTime) {
	    int time = (int) floatTime;
	    int minutes = time / 60;
	    int seconds = time % 60;
	    m_time.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

	// Power //

	public void SetLeftJoysticValue(ThrottleState value) {
		switch (value) {
			case ThrottleState.Off:
				m_powerIndicator.fillAmount = 0;
				break;
			case ThrottleState.Forward:
				m_powerIndicator.fillAmount = 0.5f;
				m_powerIndicator.fillOrigin = 1;
				break;
			case ThrottleState.Backward:
				m_powerIndicator.fillAmount = 0.5f;
				m_powerIndicator.fillOrigin = 0;
				break;
		}
	}

	// Charge//

	public void SetChargeButtonActive(bool active) {
		m_chargeButton.SetActive(active);
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

	public EnergyIndicator EnergyIndicator {
		get {
			return m_energyIndicator;
		}
	}

	public CargoIndicator CargoIndicator {
		get {
			return m_cargoIndicator;
		}
	}

}
