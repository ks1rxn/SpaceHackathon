using UnityEngine;

public class PlayerShipHull : MonoBehaviour {
    [SerializeField]
    private PlayerShip m_ship;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

	private float m_energyMaximum;
	private float m_energyDropPerSecond;
    private float m_energy;

	private readonly VectorPid rotationController = new VectorPid(4.244681f, 0.1f, 1.25f);
	private Vector3 m_needRotation;
	private Vector3 m_rotationSpeed;

    public void Initiate(float energyMaximum, float energyDropPerSecond) {
		m_needRotation = Vector3.zero;
        m_engineSystem.Initiate();

	    m_energyMaximum = energyMaximum;
	    m_energyDropPerSecond = energyDropPerSecond;
		m_energy = m_energyMaximum;  
    }

	public void SetFlyingParameters(float rotation, ThrottleState enginePower) {
		m_engineSystem.SetFlyingParameters(rotation, enginePower);
	}

	public void Hit(float strength) {
        m_energy -= strength;
		if (m_energy <= 0) {
			m_ship.Die();
		}
    }

	public void Heal(float hp) {
		m_energy += hp;
		if (m_energy > m_energyMaximum) {
			m_energy = m_energyMaximum;
		}
	}

    public void UpdateHull() {
        UpdateEnergy();
		UpdateRotation();
    }

    private void UpdateEnergy() {
		Hit(m_energyDropPerSecond * Time.fixedDeltaTime);
        BattleContext.GUIManager.PlayerGUIController.SetEnergy(m_energy / m_energyMaximum);
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
