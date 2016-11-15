using UnityEngine;

public class Blaster : MonoBehaviour {
	private float m_angle;
	private float m_detonatorActivateTime;

	public void Spawn(Vector3 position, float angle) {
		gameObject.SetActive(true);

		m_angle = angle;
		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 1, 0), m_angle);

		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
		GetComponent<TrailRenderer>().Clear();
	}

	public void UpdateBullet() {
		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * 0.4f;

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position);
		if (distToPlayer > 20) {
			Die();
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= 0.02f;
		}
	}

	private void OnTriggerEnter(Collider other) { 
		BattleContext.ExplosionsController.BlasterExplosion(transform.position);
		Die();
    }

	private void Die() {
		gameObject.SetActive(false);
	}

}
