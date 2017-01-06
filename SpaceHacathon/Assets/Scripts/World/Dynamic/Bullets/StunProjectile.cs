using UnityEngine;

public class StunProjectile : MonoBehaviour {
	private float m_angle;
	private float m_detonatorActivateTime;

	[SerializeField]
	private CollisionDetector m_collisionDetector;
	[SerializeField]
	private TrailRenderer m_trail1;
	[SerializeField]
	private TrailRenderer m_trail2;

	public void Initiate() {
		m_collisionDetector.Initiate();
		m_collisionDetector.RegisterDefaultListener(OnTargetHit);

		IsAlive = false;
	}

	public void Spawn(Vector3 position, float angle) {
		IsAlive = true;

		m_angle = angle;
		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 1, 0), m_angle);

		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
		m_trail1.Clear();
		m_trail2.Clear();
	}

	public void UpdateBullet() {
		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * 10 * Time.fixedDeltaTime;

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.Position, transform.position);
		if (distToPlayer > 20) {
			IsAlive = false;
		}

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
