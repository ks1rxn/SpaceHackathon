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

	public void Charge() {
		m_chargeFuel = 0;
		foreach (BlasterShip ship in BattleContext.EnemiesController.BlasterShips) {
			if (IsOnTarget(ship.gameObject)) {
				ship.Die();
			}
		}
		foreach (RocketShip ship in BattleContext.EnemiesController.RocketShips) {
			if (IsOnTarget(ship.gameObject)) {
				ship.Die();
			}
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
			ship.IsOnTarget(IsOnTarget(ship.gameObject));
		}
		foreach (RocketShip ship in BattleContext.EnemiesController.RocketShips) {
			ship.IsOnTarget(IsOnTarget(ship.gameObject));
		}
	}

	private bool IsOnTarget(GameObject ship) {
		if (Vector3.Distance(ship.transform.position, BattleContext.PlayerShip.transform.position) > 7.5f) {
			return false;
		}
		float angle = MathHelper.AngleBetweenVectors(BattleContext.PlayerShip.LookVector, ship.transform.position - BattleContext.PlayerShip.transform.position);
		if (Mathf.Abs(angle) < 30) {
			return true;
		}
		return false;
	}

	public bool InChargeTargeting {
		get {
			return m_chargeFuel == 5;
		}
	}

}
