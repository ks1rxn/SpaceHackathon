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

	[SerializeField]
	private Transform m_chargeOwn;
	[SerializeField]
	private Transform m_chargeTarget;

	protected void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		m_health = 1.0f;
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

	protected void Update() {
		if (m_health <= 0) {
			m_state = ShipState.OnMove;
			SceneManager.LoadScene("BattleScene");
		}
		if (m_health < 1) {
			m_health += Time.deltaTime * 0.1f;
		}

		HandleInput();
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
				}
				break;
			case ShipState.OnCharge:
				m_rigidbody.AddTorque(new Vector3(0, (AngleToTarget - m_rigidbody.angularVelocity.y * 50f) * 7.5f * Time.deltaTime, 0));
				if (m_rigidbody.angularVelocity.magnitude > 2.8f) {
					m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 2.8f;
				}
				break;
			case ShipState.OnChargeStartFly:
				m_rigidbody.angularVelocity = new Vector3();
				m_rigidbody.AddForce(LookVector * 10000);
				m_chargeTime = 0.3f;
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

	private void HandleInput() {
//		foreach (Touch touch in Input.touches) {
//			if (touch.position.x)
//		}
		switch (m_state) {
			case ShipState.OnMove:
				if (Input.GetKey(KeyCode.W)) {
					m_power = 1.0f;
				} else if (Input.GetKey(KeyCode.S)) {
					m_power = -1.0f;
				} else {
					m_power = 0;
				}

				if (Input.GetMouseButton(0)) {
					m_angle = MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), Input.mousePosition - m_button.transform.position);
				}

				if (Input.GetKeyDown(KeyCode.Space)) {
					m_state = ShipState.TransferToCharge;
				}
				break;
			case ShipState.TransferToCharge:
				break;
			case ShipState.OnCharge:
				if (Input.GetMouseButton(0)) {
					m_angle = MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), Input.mousePosition - m_button.transform.position);
				}
				if (!Input.GetKey(KeyCode.Space)) {
					m_state = ShipState.OnChargeStartFly;
				}
				break;
			case ShipState.OnChargeStartFly:
				break;
		}
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
