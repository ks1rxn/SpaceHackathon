using UnityEngine;

public class SlowingCloud : MonoBehaviour {

	public void Initiate() {
		IsAlive = false;
	}

	public void Spawn(Vector3 position) {
		IsAlive = true;
		transform.position = position;
	}

	public void UpdateState() {
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
