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
		m_chargeIndicator.SetActive(false);

	    foreach (ParticleSystem engine in m_chargeEngines) {
			engine.Stop();
			engine.Clear();
		}
		foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
			enginesAsObject.SetActive(false);
		}
    }

	public void AddFuel() {
		if (m_chargeFuel < 5) {
			m_chargeFuel++;
			UpdateChargeIndicators();
			if (InChargeTargeting) {
				m_chargeIndicator.SetActive(true);
				foreach (ParticleSystem engine in m_chargeEngines) {
					engine.Play();
				}
				foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
					enginesAsObject.SetActive(true);
				}
			}
		}
	}

	private void UpdateChargeIndicators() {
		m_chargeIndicators[0].SetActive(m_chargeFuel >= 1);
		m_chargeIndicators[1].SetActive(m_chargeFuel >= 2);
		m_chargeIndicators[2].SetActive(m_chargeFuel >= 3);
		m_chargeIndicators[3].SetActive(m_chargeFuel >= 4);
		m_chargeIndicators[4].SetActive(m_chargeFuel >= 5);
	}

	public void Charge() {
		if (!InChargeTargeting) {
			return;
		}
		m_chargeFuel -= 1;
		UpdateChargeIndicators();
		m_chargeIndicator.SetActive(false);
		foreach (ParticleSystem engine in m_chargeEngines) {
			engine.Stop();
			engine.Clear();
		}
		foreach (GameObject enginesAsObject in m_chargeEnginesAsObjects) {
			enginesAsObject.SetActive(false);
		}
//		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
//			if (IsOnTarget(ship.Position)) {
//				ship.Kill();
//			}
//		}
	}

	private void Update() {
		BattleContext.GUIController.SetCharge(m_chargeFuel / 5.0f);
		if (InChargeTargeting) {
//			int onTarget = MarkTargets();
//			switch (onTarget) {
//				case 0:
//					BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
//					break;
//				case 1:
//					BattleContext.World.SetTimeScaleMode(TimeScaleMode.Slow);
//					break;
//				default:
//					BattleContext.World.SetTimeScaleMode(TimeScaleMode.SuperSlow);
//					break;
//			}
		}
	}

	private static int MarkTargets() {
		int count = 0;
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (IsOnTarget(ship.Position)) {
				ship.CheckAsTarget();
				count++;
			} else {
				ship.UncheckAsTarget();
			}
		}
		return count;
	}

	public static List<IEnemyShip> GetTargets() {
		List<IEnemyShip> targets = new List<IEnemyShip>();
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (IsOnTarget(ship.Position)) {
				targets.Add(ship);
			}
		}
		return targets;
	} 

	private static bool IsOnTarget(Vector3 position) {
		if (Vector3.Distance(position, BattleContext.PlayerShip.Position) > 7.5f) {
			return false;
		}
		float angle = MathHelper.AngleBetweenVectors(BattleContext.PlayerShip.LookVector, position - BattleContext.PlayerShip.Position);
		if (Mathf.Abs(angle) < 30) {
			return true;
		}
		return false;
	}

	public bool InChargeTargeting {
		get {
			return m_chargeFuel > 0;
		}
	}

	public GameObject ChargeEffect {
		get {
			return m_chargeEffect;
		}
	}

}
