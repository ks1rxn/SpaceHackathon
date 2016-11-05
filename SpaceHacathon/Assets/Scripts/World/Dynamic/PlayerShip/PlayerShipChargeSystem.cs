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
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (IsOnTarget(ship.Position)) {
				ship.Kill();
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

	private static void MarkTargets() {
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (IsOnTarget(ship.Position)) {
				ship.CheckAsTarget();
			} else {
				ship.UncheckAsTarget();
			}
		}
	}

	private static bool IsOnTarget(Vector3 position) {
		if (Vector3.Distance(position, BattleContext.PlayerShip.transform.position) > 7.5f) {
			return false;
		}
		float angle = MathHelper.AngleBetweenVectors(BattleContext.PlayerShip.LookVector, position - BattleContext.PlayerShip.transform.position);
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
