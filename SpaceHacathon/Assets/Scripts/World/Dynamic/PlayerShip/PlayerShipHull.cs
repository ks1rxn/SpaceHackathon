using UnityEngine;

public class PlayerShipHull : MonoBehaviour {
	private SettingsPlayerShip m_settings;

	[SerializeField]
    private PlayerShip m_ship;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

	public float Energy;
	public int Cargo;

	private readonly VectorPid rotationController = new VectorPid(4.244681f, 0.1f, 1.25f);
	private Vector3 m_needRotation;
	private Vector3 m_rotationSpeed;

    public void Initiate() {
	    m_settings = BattleContext.Settings.PlayerShip;

		m_needRotation = Vector3.zero;
        m_engineSystem.Initiate();

		Energy = m_settings.EnergyMaximumInitial;  
		BattleContext.BattleManager.GUIManager.PlayerGUIController.CargoIndicator.SetCargoFill(Cargo);
    }

	public void SetFlyingParameters(float rotation, ThrottleState enginePower) {
		m_engineSystem.SetFlyingParameters(rotation, enginePower);
	}

	public void SpendEnergy(float energy) {
        Energy -= energy;
		if (Energy <= 0) {
			m_ship.Die();
		}
    }

	public void AddEnergy(float energy) {
		Energy += energy;
		if (Energy > m_settings.EnergyMaximumInitial) {
			Energy = m_settings.EnergyMaximumInitial;
		}
	}

	public void AddCargo(int cargo) {
		Cargo += cargo;
		if (Cargo > m_settings.CargoCapacity) {
			Cargo = m_settings.CargoCapacity;
		}
		BattleContext.BattleManager.GUIManager.PlayerGUIController.CargoIndicator.SetCargoFill(Cargo);
	}

	public void SpendCargo() {
		Cargo = 0;
		BattleContext.BattleManager.GUIManager.PlayerGUIController.CargoIndicator.SetCargoFill(Cargo);
	}


    public void UpdateHull() {
        UpdateEnergy();
		UpdateRotation();
    }

    private void UpdateEnergy() {
		SpendEnergy(m_settings.EnergyDropPerSecond * Time.fixedDeltaTime);
		BattleContext.BattleManager.GUIManager.PlayerGUIController.EnergyIndicator.SetEnergy(Energy / m_settings.EnergyMaximumInitial);

	    if (Energy < m_settings.EnergyMaximumInitial / 5) {
		    BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.TimeOn1Battery += Time.fixedDeltaTime;
	    } else if (Energy < m_settings.EnergyMaximumInitial / 5 * 2) {
		    BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.TimeOn2Battery += Time.fixedDeltaTime;
	    } else if (Energy < m_settings.EnergyMaximumInitial / 5 * 3) {
		    BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.TimeOn3Battery += Time.fixedDeltaTime;
	    } else if (Energy < m_settings.EnergyMaximumInitial / 5 * 4) {
		    BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.TimeOn4Battery += Time.fixedDeltaTime;
	    } else {
		    BattleContext.BattleManager.StatisticsManager.PlayerShipStatistics.TimeOn5Battery += Time.fixedDeltaTime;
	    }
    }

	public void SetRollAngle(float angle) {
		// prevent ship from showing his underwear
		const float max = 23;
		m_needRotation.x = Mathf.Clamp(angle, -max, max);
    }

	public void SetAcceleration(float acceleration) {
		m_needRotation.z = -acceleration / 30;
	}

	private void UpdateRotation() {
		Vector3 rotationCorrection = rotationController.Update(m_needRotation - MathHelper.AngleFrom360To180(transform.localEulerAngles), Time.fixedDeltaTime);
	    m_rotationSpeed += rotationCorrection * Time.fixedDeltaTime;
		transform.Rotate(m_rotationSpeed * Time.fixedDeltaTime);
	}

}
