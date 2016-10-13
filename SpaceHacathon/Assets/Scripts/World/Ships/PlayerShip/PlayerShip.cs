using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	private float m_angle;
	private float m_power;

	private ShipState m_state;
	private bool m_chargeKeyIsHeldDown;

    [SerializeField]
    private PlayerShipHull m_hull;
    [SerializeField]
    private PlayerShipChargeSystem m_chargeSystem;

	private void Awake() {
		BattleContext.PlayerShip = this;

        m_hull.Initiate();
        m_chargeSystem.Initiate();

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);
	}

	private void OnCollisionEnter(Collision collision) {
   		if (m_state == ShipState.OnChargeFly) {
		    if (collision.gameObject.GetComponent<BlasterShip>() != null) {
		        BattleContext.World.AddPoints(30);
		    } else if (collision.gameObject.GetComponent<RocketShip>() != null) {
			    BattleContext.World.AddPoints(20);    
		    } else if (collision.gameObject.GetComponent<Rocket>() != null) {
		        BattleContext.World.AddPoints(40);  
		    }
   		    return;
		}
		if (collision.gameObject.GetComponent<Blaster>() != null) {
			m_hull.Hit(0.2f);
		} else if (collision.gameObject.GetComponent<Rocket>() != null) {
			m_hull.Hit(0.5f);
		} else {
			m_hull.TakeDown();
		}
	}

    public void Die() {
        if (m_state == ShipState.Dead) {
            return;
        }
        m_state = ShipState.Dead;
        BattleContext.GUIController.SetDeadScore(BattleContext.World.Points);
        StartCoroutine(DieProcess());
    }

    private IEnumerator DieProcess() {
        float a = 0;
        while (true) {
            BattleContext.GUIController.SetDeadPanelOpacity(a);
            a += Time.deltaTime * 0.5f;
            if (a > 1) {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("BattleScene");
    }

	private void Update() {
		UpdateMovement();

        m_hull.EngineSystem.SetFlyingParameters(m_state, m_rigidbody.angularVelocity.y, m_power);
		BattleContext.GUIController.SetRightJoystickAngle(m_angle);
	    BattleContext.GUIController.SetLeftJoysticValue(m_power);
	}

	private void UpdateMovement() {
		switch (m_state) {
			case ShipState.OnMove:
				if ((m_chargeKeyIsHeldDown) && (m_chargeSystem.ChargeFuel > 0.3f)) {
					m_hull.Roll(0);
					m_state = ShipState.TransferToChargeTargeting;
					m_chargeSystem.PlayStartCharge(LookAngle);
                    BattleContext.World.TurnSlowMode(true);
					break;
				}

                // Rotation //
				float longAngle = -Mathf.Sign(AngleToTarget) * (360 - Mathf.Abs(AngleToTarget));
				float actualAngle = AngleToTarget;
				if ((m_rigidbody.angularVelocity.y * longAngle > 0) && (Mathf.Abs(m_rigidbody.angularVelocity.y) > 1)) {
				    if (Mathf.Abs(m_rigidbody.angularVelocity.y * 30) > Mathf.Abs(longAngle + AngleToTarget)) {
				        actualAngle = longAngle;
				    }
				}
				float angularForce = Mathf.Sign(actualAngle) * Mathf.Sqrt(Mathf.Abs(actualAngle)) * 70f;
				m_rigidbody.AddTorque(new Vector3(0, angularForce * m_rigidbody.mass * Time.deltaTime, 0));
				
                // Velocity //
				float powerCoefficient = 0;
				if (m_power > 0) {
				    powerCoefficient = 1;
				} else if (m_power < 0) {
				    powerCoefficient = -1;
				}
				m_rigidbody.AddForce(m_power * LookVector * m_rigidbody.mass * Time.deltaTime * 900);
				if (m_rigidbody.velocity.magnitude > 5) {
				    m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
				}

                // Roll hull //
		        m_hull.Roll(-m_rigidbody.angularVelocity.y * 15 * powerCoefficient);
		 
				break;
			case ShipState.TransferToChargeTargeting:
				if ((m_rigidbody.velocity.magnitude > 0.1f) || (m_rigidbody.angularVelocity.magnitude > 0.1f)) {
					m_rigidbody.velocity /= 8;
					m_rigidbody.angularVelocity /= 8;
				} else {
					m_rigidbody.velocity = new Vector3();
					m_rigidbody.angularVelocity = new Vector3();
					m_angle += AngleToTarget;

					m_state = ShipState.OnChargeTargeting;
                    m_chargeSystem.StartChargeTargeting();
				}
				break;
			case ShipState.OnChargeTargeting:
                m_chargeSystem.RotateChargeTarget(-m_angle + LookAngle);
				m_chargeSystem.ChargeTargetingMinTime -= Time.deltaTime;
				if ((m_chargeSystem.ChargeFuel <= 0) || ((!m_chargeKeyIsHeldDown) && (m_chargeSystem.ChargeTargetingMinTime <= 0))) {
                    m_chargeSystem.StopChargeTargeting();
					m_rigidbody.angularVelocity = new Vector3();
				    m_rigidbody.AddForce(LookVector * m_rigidbody.mass * 35000);
				    m_chargeSystem.ChargeTime = 0.06f;
				    m_state = ShipState.OnChargeFly;
				}
				transform.Rotate(new Vector3(0, 1, 0), (AngleToTarget * AngleToTarget * AngleToTarget * 0.000001f + AngleToTarget * 2) * Time.deltaTime);
				break;
			case ShipState.OnChargeFly:
				m_chargeSystem.ChargeTime -= Time.deltaTime;
				if (m_chargeSystem.ChargeTime <= 0) {
                    BattleContext.World.TurnSlowMode(false);
					m_state = ShipState.OnExitCharge;
				}
				break;
			case ShipState.OnExitCharge:
				if (m_rigidbody.velocity.magnitude > 50.0f) {
					m_rigidbody.velocity /= 8;
				} else {
					m_state = ShipState.OnMove;
				}
				break;
		}
	}

	public void SetAngle(float angle) {
		switch (m_state) {
			case ShipState.OnMove:
				m_angle = angle;
				break;
			case ShipState.OnChargeTargeting:
				m_angle = angle;
				break;
		}
	}

	public void SetPower(float power) {
		power = Mathf.Clamp(power, -1, 1);
		switch (m_state) {
			case ShipState.OnMove:
				m_power = power;
				break;
		}
	}

	public void StartChargeTargeting() {
		m_chargeKeyIsHeldDown = true;
	}

	public void StopChargeTargeting() {
		m_chargeKeyIsHeldDown = false;
	}

	private float LookAngle {
		get {
			return -transform.rotation.eulerAngles.y;
		}
	}

	private Vector3 LookVector {
		get {
			return new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		}
	}

	private float AngleToTarget  {
		get {
			Vector3 vectorToTarget = new Vector3(Mathf.Cos(m_angle * Mathf.PI / 180), 0, Mathf.Sin(m_angle * Mathf.PI / 180));
			return MathHelper.AngleBetweenVectors(LookVector, vectorToTarget);
		}
	}

}

public enum ShipState {
	OnMove = 0,
	TransferToChargeTargeting = 1,
	OnChargeTargeting = 2,
	OnChargeFly = 3,
	OnExitCharge = 4,
    Dead = 5
}
