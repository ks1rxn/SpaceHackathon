﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {
	private Action<GameObject> m_defaultCallback;
	private IDictionary<string, Action<GameObject>> m_callbacks;
	private IDictionary<string, Action<GameObject>> m_stayCallbacks;

	public void Initiate() {
		m_callbacks = new Dictionary<string, Action<GameObject>>();
		m_stayCallbacks = new Dictionary<string, Action<GameObject>>();
	}

	public void RegisterListener(string listenerTag, Action<GameObject> callback) {
		m_callbacks.Add(new KeyValuePair<string, Action<GameObject>>(listenerTag, callback));
	}

	public void RegisterStayListener(string listenerTag, Action<GameObject> callback) {
		m_stayCallbacks.Add(new KeyValuePair<string, Action<GameObject>>(listenerTag, callback));
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

	private void OnCollisionEnter(Collision collision) {
		Action<GameObject> callback;
		if (m_callbacks.TryGetValue(collision.gameObject.tag, out callback)) {
			callback(collision.gameObject);
			return;
		}
		if (m_defaultCallback != null) {
			m_defaultCallback(collision.gameObject);
		}
	}

	private void OnTriggerStay(Collider other) {
		Action<GameObject> callback;
		if (m_stayCallbacks.TryGetValue(other.gameObject.tag, out callback)) {
			callback(other.gameObject);
		}
	}

}

public class CollisionTags {
	public static readonly string PlayerShip = "Player";
	public static readonly string StunProjectile = "StunProjectile";
	public static readonly string Missile = "Missile";
	public static readonly string CarrierRocket = "CarrierRocket";
	public static readonly string Laser = "Laser";
	public static readonly string DroneCarrier = "DroneCarrier";
	public static readonly string RocketShip = "RocketShip";
	public static readonly string StunShip = "StunShip";
	public static readonly string RamShip = "RamShip";
	public static readonly string SpaceMine = "SpaceMine";
	public static readonly string ChargeFuel = "ChargeFuel";
	public static readonly string TimeBonus = "TimeBonus";
	public static readonly string SlowingCloud = "SlowingCloud";
}