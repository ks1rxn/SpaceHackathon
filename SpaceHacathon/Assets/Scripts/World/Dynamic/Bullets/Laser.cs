using UnityEngine;

public class Laser : MonoBehaviour {
	private float m_angle;
	private float m_detonatorActivateTime;
	private float m_lifetime;

	[SerializeField]
	private CollisionDetector m_collisionDetector;
	[SerializeField]
	private TrailRenderer m_trail;

	public void Initiate() {
		m_collisionDetector.RegisterDefaultListener(OnTargetHit);

		IsAlive = false;
	}

	public void Spawn(Vector3 position, float angle) {
		IsAlive = true;

		m_angle = angle;
		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 1, 0), m_angle);

		m_trail.Clear();

		m_lifetime = 0.5f + (float)MathHelper.Random.NextDouble() * 0.2f;
		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
	}

	public void UpdateBullet() {
		m_lifetime -= Time.fixedDeltaTime;
		if (m_lifetime < 0) {
			IsAlive = false;
		}

		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * 20 * Time.fixedDeltaTime;

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.fixedDeltaTime;
		}
	}

	private void OnTargetHit(GameObject other) {
		BattleContext.ExplosionsController.BlasterExplosion(transform.position);
		IsAlive = false;
	}

	public bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
		}
	}

}
