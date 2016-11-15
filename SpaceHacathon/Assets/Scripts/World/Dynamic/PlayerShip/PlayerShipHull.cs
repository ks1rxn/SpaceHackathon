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

    public void Roll(float angle) {
        m_needRoll = angle;
    }

    public void Hit(float strength) {
        m_health -= strength;
    }

    public void TakeDown() {
        BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
        m_health = 0;
    }

    private void FixedUpdate() {
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

    private void UpdateRolling() {
		float delta = 10;
		if (m_needRoll.Equals(0)) {
			delta = 1;
		}
		if (Mathf.Abs(m_needRoll - m_currentRoll) > delta) {
			if (m_needRoll > m_currentRoll) {
				m_currentRoll += 1.0f;
			} else {
				m_currentRoll -= 1.0f;
			}
		}

		transform.localEulerAngles = new Vector3(m_currentRoll, 0, 0);
	}

    public PlayerShipEngineSystem EngineSystem {
        get {
            return m_engineSystem;
        }
    }
}
