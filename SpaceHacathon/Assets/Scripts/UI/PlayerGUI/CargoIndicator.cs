using UnityEngine;
using UnityEngine.UI;

public class CargoIndicator : MonoBehaviour {
	[SerializeField]
	private Image m_top, m_middle, m_bottom;

	public void SetCargoFill(int fill) {
		int full = fill / 3;
		int drob = fill % 3;

		Vector2 size = m_top.rectTransform.sizeDelta;
		size.x = 58 * (full + (drob > 0 && fill < 25 ? 1 : 0));
		m_top.rectTransform.sizeDelta = size;

		size = m_middle.rectTransform.sizeDelta;
		size.x = 58 * (full + (drob > 1 || fill == 25 ? 1 : 0));
		m_middle.rectTransform.sizeDelta = size;

		size = m_bottom.rectTransform.sizeDelta;
		size.x = 58 * full;
		m_bottom.rectTransform.sizeDelta = size;
	}

}
