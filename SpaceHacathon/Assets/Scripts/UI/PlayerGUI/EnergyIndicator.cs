using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicator : MonoBehaviour {
	[SerializeField]
	private Image[] m_mainColors;
	[SerializeField]
	private Image[] m_blinkColors;

	public void SetEnergy(float energy) {
		int energyInt = Mathf.RoundToInt(energy * 100);
		int div = energyInt / 20;
		float ost = (int)((energyInt % 20) / 20.0f * 5) * 0.2f;
		float ost2 = ((int)((energyInt % 20) / 20.0f * 5) + 1) * 0.2f;
		Debug.Log(energyInt + " " + div + " " + ost);
		for (int i = 0; i < 5; i++) {
			if (i > div) {
				m_mainColors[4 - i].gameObject.SetActive(false);
				m_blinkColors[4 - i].gameObject.SetActive(false);
			} else {
				m_mainColors[4 - i].gameObject.SetActive(true);
				if (i == div) {
					m_mainColors[4 - i].fillAmount = ost;
					m_blinkColors[4 - i].gameObject.SetActive(true);
					m_blinkColors[4 - i].fillAmount = ost2;
				} else {
					m_mainColors[4 - i].fillAmount = 1.0f;
					m_blinkColors[4 - i].gameObject.SetActive(false);
				}
			}
		}
	}

	private void Update() {
		
	}

}
