using UnityEngine;

public class ChargeFuel : MonoBehaviour {
	[SerializeField]
	private CollisionDetector m_collisionDetector;

	public void Initiate() {
		m_collisionDetector.Initiate();
		m_collisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);

		IsAlive = false;
	}

	public void Spawn(Vector3 position) {
		IsAlive = true;
		transform.position = position;
	}

	public void UpdateState() {
	}

	private void OnTargetHit(GameObject other) {
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
