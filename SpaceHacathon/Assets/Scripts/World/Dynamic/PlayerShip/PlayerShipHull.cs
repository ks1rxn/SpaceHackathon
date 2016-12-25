using UnityEngine;

public class PlayerShipHull : MonoBehaviour {
    [SerializeField]
    private PlayerShip m_ship;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

    private float m_health;

	private float m_needRoll = 0;
	private float m_rollSpeed = 0;
	private readonly FloatPid rollingController = new FloatPid(4.244681f, 1.1f, 2.1f);

	private float m_needAlt = 0;
	private float m_speedAlt = 0;
	private readonly FloatPid altController = new FloatPid(4.244681f, 0.1f, 2.1f);

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
        UpdateHealth();
        UpdateRolling();
		UpdateAltitude();
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

    private void UpdateRolling() {
		float rollingCorrection = rollingController.Update(m_needRoll - MathHelper.AngleFrom360To180(transform.localEulerAngles.x), Time.fixedDeltaTime);
	    m_rollSpeed += rollingCorrection * Time.fixedDeltaTime;
		transform.Rotate(m_rollSpeed * Time.fixedDeltaTime, 0, 0);
    }

	private void UpdateAltitude() {
		float altCorrection = altController.Update(m_needAlt - transform.localPosition.y, Time.fixedDeltaTime);
	    m_speedAlt += altCorrection * Time.fixedDeltaTime;
		transform.Translate(0, m_speedAlt * Time.fixedDeltaTime, 0);
	}

}
