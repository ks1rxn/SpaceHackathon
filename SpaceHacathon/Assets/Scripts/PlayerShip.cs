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

	protected void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);
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
	}

}
