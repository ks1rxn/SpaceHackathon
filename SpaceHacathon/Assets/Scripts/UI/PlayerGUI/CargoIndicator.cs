﻿using UnityEngine;
using UnityEngine.UI;

public class CargoIndicator : MonoBehaviour {
	[SerializeField]
	private Image m_top, m_middle, m_bottom;

	private const float m_topW = 0.142f, m_midW = 0.13f, m_botW = 0.19f;

	public void SetCargoFill(int fill) {
		int full = fill / 3;
		int drob = fill % 3;
		m_top.fillAmount = m_topW * (full + (drob > 0 ? 1 : 0));
		m_middle.fillAmount = m_midW * (full + (drob > 1 ? 1 : 0));
		m_bottom.fillAmount = m_botW * full;
	}

}
