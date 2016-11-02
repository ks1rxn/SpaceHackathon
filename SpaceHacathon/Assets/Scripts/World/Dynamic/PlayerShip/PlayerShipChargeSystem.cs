using UnityEngine;

public class PlayerShipChargeSystem : MonoBehaviour {
	[SerializeField]
	private GameObject m_chargeIndicator;

    private int m_chargeFuel;
    private bool m_chargeTargeting;

    public void Initiate() {
	    m_chargeFuel = 0;
		m_chargeIndicator.SetActive(false);
    }

	public void AddFuel() {
		if (m_chargeFuel < 5) {
			m_chargeFuel++;
		}
	}

	private void Update() {
		BattleContext.GUIController.SetCharge(m_chargeFuel / 5.0f);
		m_chargeIndicator.SetActive(InChargeTargeting);
		if (InChargeTargeting) {
			MarkTargets();	
		}
	}

	private void MarkTargets() {
		foreach (BlasterShip ship in BattleContext.EnemiesController.BlasterShips) {
			if (Vector3.Distance(ship.transform.position, BattleContext.PlayerShip.transform.position) > 7.5f) {
				ship.IsOnTarget(false);
				continue;
			}
			float angle = MathHelper.AngleBetweenVectors(BattleContext.PlayerShip.LookVector, ship.transform.position - BattleContext.PlayerShip.transform.position);
			if (Mathf.Abs(angle) < 30) {
				ship.IsOnTarget(true);
			} else {
				ship.IsOnTarget(false);
			}
		}
		foreach (RocketShip ship in BattleContext.EnemiesController.RocketShips) {
			if (Vector3.Distance(ship.transform.position, BattleContext.PlayerShip.transform.position) > 7.5f) {
				ship.IsOnTarget(false);
				continue;
			}
			float angle = MathHelper.AngleBetweenVectors(BattleContext.PlayerShip.LookVector, ship.transform.position - BattleContext.PlayerShip.transform.position);
			if (Mathf.Abs(angle) < 30) {
				ship.IsOnTarget(true);
			} else {
				ship.IsOnTarget(false);
			}
		}
	}

	public bool InChargeTargeting {
		get {
			return m_chargeFuel == 5;
		}
	}

}
