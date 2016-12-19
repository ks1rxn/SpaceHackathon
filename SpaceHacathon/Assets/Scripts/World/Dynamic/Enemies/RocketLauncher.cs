using System;
using System.Collections;
using UnityEngine;

public class RocketLauncher : MonoBehaviour, IEnemyShip {
	private Rigidbody m_rigidbody;

	private float m_cooldown;

	[SerializeField]
	private float m_globalCooldownValue;

	[SerializeField]
	private Transform m_launcher;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, float angle) {
		transform.position = position;

		m_cooldown = 0;
	}

	public void Kill() {
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
		Die();
	}

	private void Die() {
		BattleContext.EnemiesController.Respawn(this);
	}

	private void OnTriggerEnter(Collider other) { 
		if (other.GetComponent<PlayerShip>() != null) {
			Kill();
		}
    }

	public void UpdateShip() {
		if (m_rigidbody.velocity.magnitude > 0.01f) {
			m_rigidbody.AddForce(-m_rigidbody.velocity * 2);
		}

		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) < 20) {
			if (m_cooldown <= 0) {
				SpawnRocket();
				m_cooldown = m_globalCooldownValue;
			}
		}

		m_cooldown -= Time.fixedDeltaTime;

		if (Vector3.Distance(BattleContext.PlayerShip.Position, transform.position) > 80) {
			Die();
		}
	}

	private void SpawnRocket() {
		BattleContext.BulletsController.SpawnRocket(m_launcher.position);

//		m_rigidbody.AddExplosionForce(120, m_launcher1.position, 3);
//		m_launcher.GetComponent<ParticleSystem>().Play();
//		m_rigidbody.AddTorque(0, 30, 0);
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}
