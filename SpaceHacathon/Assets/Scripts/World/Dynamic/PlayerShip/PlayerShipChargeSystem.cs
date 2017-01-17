using System.Collections.Generic;
using UnityEngine;

public class PlayerShipChargeSystem : MonoBehaviour {
	[SerializeField]
	private GameObject m_chargeIndicator;
	[SerializeField]
	private ParticleSystem[] m_chargeEngines;
	[SerializeField]
	private GameObject[] m_chargeEnginesAsObjects;
	[SerializeField]
	private GameObject m_chargeEffect;
	[SerializeField]
	private GameObject[] m_chargeIndicators;

    private int m_chargeFuel;

    public void Initiate() {
	    m_chargeFuel = 0;
		UpdateChargeIndicators();
//		UpdateChargeEngines();
    }

	public void AddFuel() {
		if (m_chargeFuel < 5) {
			m_chargeFuel++;
			UpdateChargeIndicators();
//			UpdateChargeEngines();
		}
	}

	public void Charge() {
//		if (!InChargeTargeting) {
//			return;
//		}
//		PlayerShipHull hull = BattleContext.PlayerShip.Hull;
//		if (hull.TryToGetFuel(0.2f)) {
//			
//		}
//		UpdateChargeIndicators();
//		UpdateChargeEngines();
	}

	private void UpdateChargeIndicators() {
		BattleContext.GUIController.SetCharge(m_chargeFuel);
		BattleContext.GUIController.SetChargeButtonActive(m_chargeFuel > 0);

		m_chargeIndicators[0].SetActive(m_chargeFuel >= 1);
		m_chargeIndicators[1].SetActive(m_chargeFuel >= 2);
		m_chargeIndicators[2].SetActive(m_chargeFuel >= 3);
		m_chargeIndicators[3].SetActive(m_chargeFuel >= 4);
		m_chargeIndicators[4].SetActive(m_chargeFuel >= 5);
	}

//	private void UpdateChargeEngines() {
//		if (InChargeTargeting) {
//			m_chargeIndicator.SetActive(true);
//			foreach (ParticleSystem engine in m_chargeEngines) {
//				engine.Play();
//			}
//			foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
//				enginesAsObject.SetActive(true);
//			}
//		} else {
//			m_chargeIndicator.SetActive(false);
//			foreach (ParticleSystem engine in m_chargeEngines) {
//				engine.Stop();
//				engine.Clear();
//			}
//			foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
//				enginesAsObject.SetActive(false);
//			}
//		}
//	}

//	public bool InChargeTargeting {
//		get {
//			return m_chargeFuel > 0;
//		}
//	}

	public GameObject ChargeEffect {
		get {
			return m_chargeEffect;
		}
	}

}
