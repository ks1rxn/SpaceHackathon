﻿using UnityEngine;

public class PlayerShipChargeSystem : MonoBehaviour {
	[SerializeField]
	private GameObject m_chargeIndicator;
	[SerializeField]
	private ParticleSystem[] m_chargeEngines;

    private int m_chargeFuel;

    public void Initiate() {
	    m_chargeFuel = 0;
		m_chargeIndicator.SetActive(false);
    }

	public void AddFuel() {
		if (m_chargeFuel < 5) {
			m_chargeFuel++;
			if (InChargeTargeting) {
				m_chargeIndicator.SetActive(true);
				foreach (ParticleSystem engine in m_chargeEngines) {
					engine.Play();
				}
			}
		}
	}

	public void Charge() {
		m_chargeFuel = 0;
		m_chargeIndicator.SetActive(false);
		foreach (ParticleSystem engine in m_chargeEngines) {
			engine.Stop();
			engine.Clear();
		}
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (IsOnTarget(ship.Position)) {
				ship.Kill();
			}
		}
	}

	private void Update() {
		BattleContext.GUIController.SetCharge(m_chargeFuel / 5.0f);
		if (InChargeTargeting) {
			int onTarget = MarkTargets();
			switch (onTarget) {
				case 0:
					BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
					break;
				case 1:
					BattleContext.World.SetTimeScaleMode(TimeScaleMode.Slow);
					break;
				default:
					BattleContext.World.SetTimeScaleMode(TimeScaleMode.SuperSlow);
					break;
			}
		} else {
			BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
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
