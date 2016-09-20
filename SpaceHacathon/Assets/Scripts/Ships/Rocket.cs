﻿using UnityEngine;

public class Rocket : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private float m_lifeTime;
	private float m_detonatorActivateTime;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, float lifeTime) {
		transform.position = position;
		m_lifeTime = lifeTime;

		m_detonatorActivateTime = 0.1f;
		GetComponent<Collider>().enabled = false;

		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		m_rigidbody.AddForce(lookVector * 50);
	}

	protected void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
		BattleContext.ExplosionsController.RocketExplosion(transform.position);
    }

	protected void Update() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		m_rigidbody.AddTorque(new Vector3(0, (angle - m_rigidbody.angularVelocity.y * 50) * 0.1f * Time.deltaTime, 0));
		if (m_rigidbody.angularVelocity.magnitude > 5) {
			m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 5;
		}

		m_rigidbody.AddForce(lookVector.normalized * 4000 * Time.deltaTime);
		if (m_rigidbody.velocity.magnitude > 6) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 6;
		}

		m_lifeTime -= Time.deltaTime;
		if (m_lifeTime <= 0) {
			Destroy(gameObject);
			BattleContext.ExplosionsController.RocketExplosion(transform.position);
		}

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position);
		if (distToPlayer > 25) {
			Destroy(gameObject);
		}

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.deltaTime;
		}
	}

}
