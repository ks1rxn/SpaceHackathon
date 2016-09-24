using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	[SerializeField]
	private GameObject m_button;

	private float m_angle;
	private bool m_slow;
	private float m_power;
	private float m_health;

	private ShipState m_state;
	private float m_chargeTime;
	private bool m_inChargeTargeting;

	[SerializeField]
	private Transform m_chargeOwn;
	[SerializeField]
	private Transform m_chargeTarget;
	[SerializeField]
	private ParticleSystem[] m_engines;
	private bool[] m_engineState;

	protected void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		m_health = 1.0f;
		m_inChargeTargeting = false;

		m_engineState = new bool[m_engines.Length];
	}

	protected void OnCollisionEnter(Collision collision) {
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

	private void SetEngineState(int engine, bool state) {
		if (m_engineState[engine] != state) {
			m_engineState[engine] = state;
			if (state) {
				m_engines[engine].Play();
			} else {
				m_engines[engine].Stop();
			}
		}
	}

	protected void Update() {
		if (m_health <= 0) {
			m_state = ShipState.OnMove;
			SceneManager.LoadScene("BattleScene");
		}
		if (m_health < 1) {
			m_health += Time.deltaTime * 0.1f;
		}

//		float rotate = AngleToTarget - m_rigidbody.angularVelocity.y * 50f;
		float rotate = m_rigidbody.angularVelocity.y;
		float powerCoef = 1;
		if (m_power < -0.1f) {
			powerCoef = -1;
		}
		if (Mathf.Abs(rotate) > 0.5f) {
			if (rotate * powerCoef < 0) {
				SetEngineState(3, true);
				SetEngineState(6, true);
				SetEngineState(4, false);
				SetEngineState(5, false);
			} else {
				SetEngineState(3, false);
				SetEngineState(6, false);
				SetEngineState(4, true);
				SetEngineState(5, true);
			}
		} else {
			SetEngineState(3, false);
			SetEngineState(6, false);
			SetEngineState(4, false);
			SetEngineState(5, false);
		}
		if (m_state == ShipState.OnMove) {
			if (m_power > 0.1f) {
				SetEngineState(0, true);
				SetEngineState(1, true);
				SetEngineState(2, true);
				SetEngineState(7, false);
				SetEngineState(8, false);
			} else if (m_power < -0.1f) {
				SetEngineState(0, false);
				SetEngineState(1, false);
				SetEngineState(2, false);
				SetEngineState(7, true);
				SetEngineState(8, true);
			} else {
				SetEngineState(0, false);
				SetEngineState(1, false);
				SetEngineState(2, false);
				SetEngineState(7, false);
				SetEngineState(8, false);
			}
		} else {
			for (int i = 0; i != m_engines.Length; i++) {
				SetEngineState(i, false);
			}
		}
		Move();
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

	private void Move() {
		switch (m_state) {
			case ShipState.OnMove:
				if (m_inChargeTargeting) {
					m_chargeTime = 0.2f;
					m_state = ShipState.TransferToCharge;
				}
				m_rigidbody.AddTorque(new Vector3(0, (AngleToTarget - m_rigidbody.angularVelocity.y * 50f) * 7.5f * Time.deltaTime, 0));
				if (m_rigidbody.angularVelocity.magnitude > 2.8f) {
					m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 2.8f;
				}

				m_rigidbody.AddForce(m_power * LookVector * Time.deltaTime * 600);

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
				m_rigidbody.AddTorque(new Vector3(0, (AngleToTarget - m_rigidbody.angularVelocity.y * 50f) * 7.5f * Time.deltaTime, 0));
				if (m_rigidbody.angularVelocity.magnitude > 2.8f) {
					m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 2.8f;
				}
				break;
			case ShipState.OnChargeStartFly:
				m_rigidbody.angularVelocity = new Vector3();
				m_rigidbody.AddForce(LookVector * 35000);
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

	private enum ShipState {
		OnMove = 0,
		TransferToCharge = 1,
		OnCharge = 2,
		OnChargeStartFly = 3,
		OnChargeFly = 4,
		OnExitCharge = 5
	}

}
