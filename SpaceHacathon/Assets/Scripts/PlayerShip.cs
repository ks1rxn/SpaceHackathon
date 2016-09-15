using UnityEngine;
using UnityEngine.UI;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	[SerializeField]
	private GameObject m_space;

	[SerializeField]
	private GameObject m_button;
	[SerializeField]
	private GameObject m_smallButton;
	[SerializeField]
	private Slider m_powerBar;

	private float m_angle;
	private bool m_slow;
	private float m_power;

	[SerializeField]
	private PlayerShipEngine[] m_engines;

	protected void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(1, 0, 0);
	}

	protected void Update() {
		if (Input.GetKey(KeyCode.W)) {
//			m_power += 0.05f;
			m_power = 1.0f;
		} else if (Input.GetKey(KeyCode.S)) {
//			m_power -= 0.05f;
			m_power = -1.0f;
		} else {
			m_power = 0;
		}
		if (m_power > 1) {
			m_power = 1;
		}
		if (m_power < -1) {
			m_power = -1;
		}

		Move();

		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			m_slow = !m_slow;
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			SetEngineParams(0, 20, 1);
			SetEngineParams(1, 20, 1);
			SetEngineParams(2, -20, 0.5f);
			SetEngineParams(3, -20, 0.5f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha7)) {
			SetEnginePower(2, 1.0f);
			SetEngineAngle(2, 0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha8)) {
			SetEnginePower(2, 1.0f);
			SetEngineAngle(2, 90);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9)) {
			SetEnginePower(2, 1.0f);
			SetEngineAngle(2, 180);
		}
		if (m_slow && Time.timeScale > 0.3f) {
			Time.timeScale -= Time.deltaTime * 5;
		}
		if (!m_slow && Time.timeScale < 1) {
			Time.timeScale += Time.deltaTime * 5;
		}
		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		m_powerBar.value = m_power / 2 + 0.5f;
	}

	private void Move() {
		if (Input.GetMouseButton(0)) {
			m_angle = MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), Input.mousePosition - m_button.transform.position);
		}
		m_smallButton.transform.localPosition = new Vector3(Mathf.Cos(m_angle * Mathf.PI / 180) * 100, Mathf.Sin(m_angle * Mathf.PI / 180) * 100, 0);
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Vector3 rotVector = new Vector3(Mathf.Cos(m_angle * Mathf.PI / 180), 0, Mathf.Sin(m_angle * Mathf.PI / 180));
		float rotAngle = MathHelper.AngleBetweenVectors(lookVector, rotVector);
		m_rigidbody.AddTorque(new Vector3(0, (rotAngle - m_rigidbody.angularVelocity.y * 50) * 75, 0));
	    if (m_rigidbody.angularVelocity.magnitude > 2.8f) {
			m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 2.8f;
	    }

		m_rigidbody.AddForce(m_power * lookVector * 6);

		if (m_rigidbody.velocity.magnitude > 5) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
		}

		float anglularSpeed = m_rigidbody.angularVelocity.y;
		bool moveToLook = Mathf.Abs(MathHelper.AngleBetweenVectors(lookVector, m_rigidbody.velocity)) < 90;
		foreach (PlayerShipEngine engine in m_engines) {
			engine.SetPower(Mathf.Abs(m_power));
		}

		float angleChanger = anglularSpeed * 15;
		if (m_power >= 0) {
			if (moveToLook) {
				SetEngineAngle(0, angleChanger);
				SetEngineAngle(1, angleChanger);
				SetEngineAngle(2, -angleChanger);
				SetEngineAngle(3, -angleChanger);
			} else {
				SetEngineAngle(0, -angleChanger);
				SetEngineAngle(1, -angleChanger);
				SetEngineAngle(2, angleChanger);
				SetEngineAngle(3, angleChanger);
			}
		} else {
			if (moveToLook) {
				SetEngineAngle(0, 180 - angleChanger);
				SetEngineAngle(1, 180 - angleChanger);
				SetEngineAngle(2, 180 + angleChanger);
				SetEngineAngle(3, 180 + angleChanger);
			} else {
				SetEngineAngle(0, 180 + angleChanger);
				SetEngineAngle(1, 180 + angleChanger);
				SetEngineAngle(2, 180 - angleChanger);
				SetEngineAngle(3, 180 - angleChanger);
			}
		}
	}

	private void SetEnginePower(int engine, float power) {
		m_engines[engine].SetPower(power);
	}

	private void SetEngineAngle(int engine, float angle) {
		m_engines[engine].SetAngle(angle);
	}

	private void SetEngineParams(int engine, float angle, float power) {
		m_engines[engine].SetPower(power);
		m_engines[engine].SetAngle(angle);
	}

}
