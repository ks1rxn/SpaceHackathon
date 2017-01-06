using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {
	private Action<GameObject> m_defaultCallback;
	private IDictionary<string, Action<GameObject>> m_callbacks;

	public void Initiate() {
		m_callbacks = new Dictionary<string, Action<GameObject>>();
	}

	public void RegisterListener(string listenerTag, Action<GameObject> callback) {
		m_callbacks.Add(new KeyValuePair<string, Action<GameObject>>(listenerTag, callback));
	}

	public void RegisterDefaultListener(Action<GameObject> callback) {
		m_defaultCallback = callback;
	}

	private void OnTriggerEnter(Collider other) {
		Action<GameObject> callback;
		if (m_callbacks.TryGetValue(other.gameObject.tag, out callback)) {
			callback(other.gameObject);
			return;
		}
		if (m_defaultCallback != null) {
			m_defaultCallback(other.gameObject);
		}
	}

}
