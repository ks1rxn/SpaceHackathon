using UnityEngine;

public class PlayerShipHull : MonoBehaviour {
    [SerializeField]
    private PlayerShip m_ship;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

    private float m_health;
	private float m_fuel;

	private readonly VectorPid rotationController = new VectorPid(4.244681f, 0.1f, 1.25f);
	private Vector3 m_needRotation;
	private Vector3 m_rotationSpeed;

    public void Initiate() {
		m_needRotation = Vector3.zero;
        m_engineSystem.Initiate();
        m_health = 1.0f;
	    m_fuel = 1.0f;
    }

	public void SetFlyingParameters(float rotation, ThrottleState enginePower) {
		m_engineSystem.SetFlyingParameters(rotation, enginePower);
	}

	public void Hit(float strength) {
        m_health -= strength;
    }

    public void UpdateHull() {
        UpdateHealth();
		UpdateRotation();
    }

    private void UpdateHealth() {
        if (m_health <= 0) {
//			m_ship.Die();
		}
		if (m_health < 1) {
			m_health += 0.1f / 50;
		}
        BattleContext.GUIController.SetHealth(m_health);
		BattleContext.GUIController.SetFuel(m_fuel);
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

	public void AddFuel(float fuel) {
		m_fuel += fuel;
		if (m_fuel > 1.0f) {
			m_fuel = 1.0f;
		}
	}

	public bool TryToGetFuel(float fuel) {
		if (m_fuel >= fuel) {
			m_fuel -= fuel;
			return true;
		} else {
			return false;
		}
	}

}
