using UnityEngine;

public class ChildrenDestroyer : MonoBehaviour {

	private void Awake() {
		foreach (Transform child in transform) {
			DestroyImmediate(child.gameObject);
		}
	}

}
