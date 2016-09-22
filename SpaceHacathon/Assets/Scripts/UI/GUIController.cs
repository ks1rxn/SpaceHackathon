using UnityEngine;
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
	private Text m_fpsCounter;

	protected void Awake() {
		BattleContext.GUIController = this;
	}

	protected void Update() {
		m_fpsCounter.text = "FPS: " + Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
	}

	public void SetRightJoystickAngle(float angle) {
		m_smallButton.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) * Screen.width / 8, Mathf.Sin(angle * Mathf.PI / 180) * Screen.width / 8, 0);
	}

	public void SetLeftJoysticValue(float value) {
		m_powerBar.value = value / 2 + 0.5f;
	}

	public void SetHealth(float value) {
		m_health.value = value;
	}

	public GameObject Button {
		get {
			return m_button;
		}
	}
}
