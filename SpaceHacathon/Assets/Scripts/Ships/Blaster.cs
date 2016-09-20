using UnityEngine;

public class Blaster : MonoBehaviour {
	private BlasterShip m_owner;
	private float m_angle;
	private float m_detonatorActivateTime;

	public void Spawn(BlasterShip owner, Vector3 position, float angle) {
		m_owner = owner;
		m_angle = angle;
		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 1, 0), m_angle);

		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
	}

	protected void Update() {
		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * Time.deltaTime * 20;

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position);
		if (distToPlayer > 20) {
			Destroy(gameObject);
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.deltaTime;
		}
	}

	protected void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
    }

}
