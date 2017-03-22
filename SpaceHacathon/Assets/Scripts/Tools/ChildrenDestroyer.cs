using System.Collections.Generic;
using UnityEngine;

public class ChildrenDestroyer : MonoBehaviour {

	private void Awake() {
		int count = transform.childCount;
		List<Transform> childs = new List<Transform>(count);
		for (int i = 0; i != count; i++) {
			Transform child = transform.GetChild(i);
			childs.Add(child);
		}
		foreach (Transform child in childs) {
			DestroyImmediate(child.gameObject);
		}
	}

}
