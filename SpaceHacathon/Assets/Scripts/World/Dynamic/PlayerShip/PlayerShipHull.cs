using UnityEngine;

public class PlayerShipHull : MonoBehaviour {
    [SerializeField]
    private PlayerShip m_ship;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

    private float m_health;

	private float m_needRoll = 0;
	private float m_rollSpeed = 0;
	private readonly FloatPid rollingController = new FloatPid(4.244681f, 1.1f, 1.9f);

	private float m_needTilt = 0;
	private float m_tiltSpeed = 0;
	private readonly FloatPid tiltController = new FloatPid(4.244681f, 0.1f, 0.75f);

	private float m_needY = 0;
	private float m_ySpeed = 0;
	private readonly FloatPid yController = new FloatPid(4.244681f, 0.1f, 1.75f);

    public void Initiate() {
        m_engineSystem.Initiate();
        m_health = 1.0f;  
    }

	public void SetFlyingParameters(float rotation, float enginePower) {
		m_engineSystem.SetFlyingParameters(rotation, enginePower);
	}

	public void Hit(float strength) {
        m_health -= strength;
    }

    public void UpdateHull() {
		//todo: change to VectorPid
        UpdateHealth();
        UpdateRolling();
		UpdateTilt();
		UpdateY();
    }

    private void UpdateHealth() {
        if (m_health <= 0) {
//			m_ship.Die();
		}
		if (m_health < 1) {
			m_health += 0.1f / 50;
		}
        BattleContext.GUIController.SetHealth(m_health);
    }

	public void SetRollAngle(float angle) {
		// prevent ship from showing his underwear
		const float max = 23;
        m_needRoll = Mathf.Clamp(angle, -max, max);
    }

	public void SetAcceleration(float acceleration) {
		m_needTilt = -acceleration / 30;
	}

	private void UpdateTilt() {
		float tiltCorrection = tiltController.Update(m_needTilt - MathHelper.AngleFrom360To180(transform.localEulerAngles.z), Time.fixedDeltaTime);
	    m_tiltSpeed += tiltCorrection * Time.fixedDeltaTime;
		transform.Rotate(0, 0, m_tiltSpeed * Time.fixedDeltaTime);
	}

	private void UpdateY() {
		float yCorrection = yController.Update(-MathHelper.AngleFrom360To180(transform.localEulerAngles.y), Time.fixedDeltaTime);
	    m_ySpeed += yCorrection * Time.fixedDeltaTime;
		transform.Rotate(0, m_ySpeed * Time.fixedDeltaTime, 0);
	}

    private void UpdateRolling() {
		float rollingCorrection = rollingController.Update(m_needRoll - MathHelper.AngleFrom360To180(transform.localEulerAngles.x), Time.fixedDeltaTime);
	    m_rollSpeed += rollingCorrection * Time.fixedDeltaTime;
		transform.Rotate(m_rollSpeed * Time.fixedDeltaTime, 0, 0);
    }

}
