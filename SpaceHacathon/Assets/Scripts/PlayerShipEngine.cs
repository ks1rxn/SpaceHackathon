using UnityEngine;

public class PlayerShipEngine : MonoBehaviour {
	[SerializeField]
	private Transform m_particles;

	private float m_needPower;
	private float m_needAngle;

	private float m_realPower;
	private float m_realAngle;

	protected void Awake() {
		m_needAngle = m_realAngle = 0;
		m_needPower = m_realPower = 0;
	}

	protected void Update() {
		if (m_realPower < m_needPower) {
			m_realPower += Time.deltaTime;
		} else if (m_realPower > m_needPower) {
			m_realPower -= Time.deltaTime;
		}
		Vector3 realVector = new Vector3(Mathf.Cos(m_realAngle * Mathf.PI / 180), 0, Mathf.Sin(m_realAngle * Mathf.PI / 180));
		Vector3 needVector = new Vector3(Mathf.Cos(m_needAngle * Mathf.PI / 180), 0, Mathf.Sin(m_needAngle * Mathf.PI / 180));
		float angle = MathHelper.AngleBetweenVectors(realVector, needVector);
		if (Mathf.Abs(angle) > 5) {
			if (!angle.Equals(0)) {
				angle /= Mathf.Abs(angle);
			}
			m_realAngle -= Time.deltaTime * 360 * angle;
		}

		PerformAngle();
		PerformPower();
	}

	private void PerformAngle() {
		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 0, 1), m_realAngle);
	}

	private void PerformPower() {
		Vector3 scale = m_particles.transform.localScale;
		float coef = 0.003f;
		scale.z = m_realPower * coef;
		m_particles.transform.localScale = scale;
	}

	public  void SetPower(float power) {
		m_needPower = power;
	}

	public void SetAngle(float angle) {
		m_needAngle = angle;
	}

}
