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
		BattleContext.BattleManager.GUIManagerObsolete.PlayerGUIController.SetChargeButtonActive(CanCharge);

		float energyForCharge = BattleContext.Settings.PlayerShip.EnergyForCharge;
		m_chargeIndicators[0].SetActive(m_ship.Hull.Energy >= energyForCharge);
		m_chargeIndicators[1].SetActive(m_ship.Hull.Energy >= energyForCharge * 2);
		m_chargeIndicators[2].SetActive(m_ship.Hull.Energy >= energyForCharge * 3);
		m_chargeIndicators[3].SetActive(m_ship.Hull.Energy >= energyForCharge * 4);
		m_chargeIndicators[4].SetActive(m_ship.Hull.Energy >= energyForCharge * 5);
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
			return m_ship.Hull.Energy >= BattleContext.Settings.PlayerShip.EnergyForCharge;
		}
	}

	public GameObject ChargeEffect {
		get {
			return m_chargeEffect;
		}
	}

}
