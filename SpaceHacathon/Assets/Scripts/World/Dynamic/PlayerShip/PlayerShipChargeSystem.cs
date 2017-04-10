using UnityEngine;

public class PlayerShipChargeSystem : MonoBehaviour {
	[SerializeField]
	private PlayerShip m_ship;

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

    public void Initiate() {
		UpdateChargeIndicators();
		UpdateChargeEngines();
    }

	public void UpdateChargeSystem() {
		UpdateChargeIndicators();
		UpdateChargeEngines();
	}

	private void UpdateChargeIndicators() {
		BattleContext.BattleManager.GUIManager.PlayerGUIController.SetChargeButtonActive(CanCharge);

		m_chargeIndicators[0].SetActive(m_ship.Hull.Energy >= 20);
		m_chargeIndicators[1].SetActive(m_ship.Hull.Energy >= 40);
		m_chargeIndicators[2].SetActive(m_ship.Hull.Energy >= 60);
		m_chargeIndicators[3].SetActive(m_ship.Hull.Energy >= 80);
		m_chargeIndicators[4].SetActive(m_ship.Hull.Energy >= 100);
	}

	private void UpdateChargeEngines() {
		if (CanCharge) {
			m_chargeIndicator.SetActive(true);
			foreach (ParticleSystem engine in m_chargeEngines) {
				engine.Play();
			}
			foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
				enginesAsObject.SetActive(true);
			}
		} else {
			m_chargeIndicator.SetActive(false);
			foreach (ParticleSystem engine in m_chargeEngines) {
				engine.Stop();
				engine.Clear();
			}
			foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
				enginesAsObject.SetActive(false);
			}
		}
	}

	public bool CanCharge {
		get {
			return m_ship.Hull.Energy >= 20;
		}
	}

	public GameObject ChargeEffect {
		get {
			return m_chargeEffect;
		}
	}

}
