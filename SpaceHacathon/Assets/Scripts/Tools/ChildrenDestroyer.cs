using UnityEngine;

public class ChildrenDestroyer : MonoBehaviour {

	private void Awake() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
	}

}
