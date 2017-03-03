using UnityEngine;

public class SlowingCloud : MonoBehaviour {
	private float m_lifeTime;

	public void Initiate() {
		IsAlive = false;
	}

	public void Spawn(Vector3 position) {
		IsAlive = true;
		transform.position = position;

		m_lifeTime = 5;
	}

	public void UpdateState() {
		m_lifeTime -= Time.fixedDeltaTime;
		if (m_lifeTime <= 0) {
			IsAlive = false;
		}
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
