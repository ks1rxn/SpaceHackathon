using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	[SerializeField]
	private GameObject m_button;
	[SerializeField]
	private GameObject m_smallButton;
	[SerializeField]
	private Slider m_powerBar;

	protected void Awake() {
		BattleContext.GUIController = this;
	}

	public void SetRightJoystickAngle(float angle) {
		m_smallButton.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.PI / 180) * 100, Mathf.Sin(angle * Mathf.PI / 180) * 100, 0);
	}

	public void SetLeftJoysticValue(float value) {
		m_powerBar.value = value / 2 + 0.5f;
	}

}
