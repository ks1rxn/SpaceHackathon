using UnityEngine;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	private float m_angle;
	private bool m_slow;
	private float m_power;
	private float m_health;
	private float m_roll;
	private float m_needRoll;

	private ShipState m_state;
	private float m_chargeTime;
	private bool m_inChargeTargeting;

	[SerializeField]
	private Transform m_ship;
	[SerializeField]
	private ParticleSystem m_chargeStartEffect;
	[SerializeField]
	private Transform m_chargeOwn;
	[SerializeField]
	private Transform m_chargeTarget;
    [SerializeField]
    private PlayerShipEngineSystem m_engineSystem;

	private void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		m_health = 1.0f;
		m_inChargeTargeting = false;

        m_engineSystem.Initiate();
	}

	private void OnCollisionEnter(Collision collision) {
		if (m_state == ShipState.OnChargeFly) {
			return;
		}
		if (collision.gameObject.GetComponent<Blaster>() != null) {
			m_health -= 0.2f;
		} else if (collision.gameObject.GetComponent<Rocket>() != null) {
			m_health -= 0.5f;
		} else {
			BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
			m_health = 0;
		}
	}

	private void Update() {
        /* if (m_health <= 0) {
			m_state = ShipState.OnMove;
			SceneManager.LoadScene("BattleScene");
		}*/
		if (m_health < 1) {
			m_health += Time.deltaTime * 0.1f;
		}

        m_engineSystem.SetFlyingParameters(m_state, m_rigidbody.angularVelocity.y, m_power);

		UpdateMovement();
		UpdateRolling();
		UpdateTimeSpeed();

		if (m_state == ShipState.OnCharge) {
			m_chargeOwn.gameObject.SetActive(true);
			m_chargeTarget.gameObject.SetActive(true);

			m_chargeTarget.rotation = new Quaternion();
			m_chargeTarget.Rotate(new Vector3(0, 1, 0), -m_angle + LookAngle);
		} else {
			m_chargeOwn.gameObject.SetActive(false);
			m_chargeTarget.gameObject.SetActive(false);
		}

		BattleContext.GUIController.SetRightJoystickAngle(m_angle);
		BattleContext.GUIController.SetLeftJoysticValue(m_power);
		BattleContext.GUIController.SetHealth(m_health);
	}

	private void UpdateMovement() {
		switch (m_state) {
			case ShipState.OnMove:
				if (m_inChargeTargeting) {
					m_chargeTime = 0.2f;
					m_needRoll = 0;
					m_state = ShipState.TransferToCharge;
					m_chargeStartEffect.startRotation = -LookAngle * Mathf.PI / 180;
					m_chargeStartEffect.Play();
					break;
				}

				float longAngle = -Mathf.Sign(AngleToTarget) * (360 - Mathf.Abs(AngleToTarget));
				float actualAngle = AngleToTarget;
				if ((m_rigidbody.angularVelocity.y * longAngle > 0) && (Mathf.Abs(m_rigidbody.angularVelocity.y) > 1)) {
					if (Mathf.Abs(m_rigidbody.angularVelocity.y * 30) > Mathf.Abs(longAngle + AngleToTarget)) {
						actualAngle = longAngle;
					}
				}
				float angularForce = Mathf.Sign(actualAngle) * Mathf.Sqrt(Mathf.Abs(actualAngle)) * 70f;
				m_rigidbody.AddTorque(new Vector3(0, angularForce * m_rigidbody.mass * Time.deltaTime, 0));

				float powerCoefficient = 0;
				if (m_power > 0) {
					powerCoefficient = 1;
				} else if (m_power < 0) {
					powerCoefficient = -1;
				}
				m_needRoll = -m_rigidbody.angularVelocity.y * 15 * powerCoefficient;
				m_rigidbody.AddForce(m_power * LookVector * m_rigidbody.mass * Time.deltaTime * 900);
				if (m_rigidbody.velocity.magnitude > 5) {
					m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
				}
				break;
			case ShipState.TransferToCharge:
				if ((m_rigidbody.velocity.magnitude > 0.1f) || (m_rigidbody.angularVelocity.magnitude > 0.1f)) {
					m_rigidbody.velocity /= 8;
					m_rigidbody.angularVelocity /= 8;
				} else {
					m_rigidbody.velocity = new Vector3();
					m_rigidbody.angularVelocity = new Vector3();
					m_angle += AngleToTarget;

					m_state = ShipState.OnCharge;
					// set time here? or in previous state?
					m_chargeTime = 0.1f;
				}
				break;
			case ShipState.OnCharge:
				// m_chargeTime in this case - time to start charge engine. Should be equal to charge engine animation length.
				m_chargeTime -= Time.deltaTime;
				if ((!m_inChargeTargeting) && (m_chargeTime <= 0)) {
					m_state = ShipState.OnChargeStartFly;
				}
				transform.Rotate(new Vector3(0, 1, 0), (AngleToTarget * AngleToTarget * AngleToTarget * 0.000001f + AngleToTarget * 2) * Time.deltaTime);
				break;
			case ShipState.OnChargeStartFly:
				m_rigidbody.angularVelocity = new Vector3();
				m_rigidbody.AddForce(LookVector * m_rigidbody.mass * 35000);
				m_chargeTime = 0.06f;
				m_state = ShipState.OnChargeFly;
				break;
			case ShipState.OnChargeFly:
				m_chargeTime -= Time.deltaTime;
				if (m_chargeTime <= 0) {
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

	private void UpdateRolling() {
		float delta = 10;
		if (m_needRoll.Equals(0)) {
			delta = 1;
		}
		if (Mathf.Abs(m_needRoll - m_roll) > delta) {
			if (m_needRoll > m_roll) {
				m_roll += Time.deltaTime * 80;
			} else {
				m_roll -= Time.deltaTime * 80;
			}
		}

		m_ship.rotation = new Quaternion();
		m_ship.Rotate(new Vector3(1, 0, 0), m_roll);
	}

	public void SetAngle(float angle) {
		switch (m_state) {
			case ShipState.OnMove:
				m_angle = angle;
				break;
			case ShipState.OnCharge:
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
		m_inChargeTargeting = true;
	}

	public void StopChargeTargeting() {
		m_inChargeTargeting = false;
	}

	private void UpdateTimeSpeed() {
		if ((m_state == ShipState.TransferToCharge) || (m_state == ShipState.OnCharge) || (m_state == ShipState.OnChargeStartFly) || (m_state == ShipState.OnChargeFly)) {
			if (Time.timeScale > 0.3f) {
				Time.timeScale -= Time.deltaTime * 5;
			}
		} else {
			if (Time.timeScale < 1) {
				Time.timeScale += Time.deltaTime * 5;
			}
		}
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
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
	TransferToCharge = 1,
	OnCharge = 2,
	OnChargeStartFly = 3,
	OnChargeFly = 4,
	OnExitCharge = 5
}
