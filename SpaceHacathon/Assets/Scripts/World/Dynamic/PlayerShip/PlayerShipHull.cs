using UnityEngine;

public class PlayerShipHull : MonoBehaviour {
    [SerializeField]
    private PlayerShip m_ship;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

	private float m_currentRoll;
	private float m_needRoll;

    private float m_health;

    public void Initiate() {
        m_currentRoll = 0;
        m_needRoll = 0;
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
        m_needRoll = angle;
    }

    private void UpdateRolling() {
		float delta = 2.0f;
		if (Mathf.Abs(m_needRoll - m_currentRoll) > delta) {
			m_currentRoll += m_needRoll > m_currentRoll ? 1.0f : -1.0f;
		}
		transform.localEulerAngles = new Vector3(m_currentRoll, 0, 0);
	}

}
