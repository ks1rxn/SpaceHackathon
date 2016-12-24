using System;
using System.Collections;
using UnityEngine;

public class RocketLauncher : IEnemyShip {
	private float m_cooldown;

	[SerializeField]
	private float m_globalCooldownValue;

	[SerializeField]
	private Transform m_launcher;

	public override void Initiate() {
		base.Initiate();
	}

	public override void Spawn(Vector3 position, float angle) {
		base.Spawn(position, angle);

		m_cooldown = 0;
	}

	public override void Kill() {
		base.Kill();
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
	}

	private void OnTriggerEnter(Collider other) { 
		if (other.GetComponent<PlayerShip>() != null) {
			Kill();
		}
    }

	public override void UpdateShip() {
		base.UpdateShip();

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
	}

	private void SpawnRocket() {
		BattleContext.BulletsController.SpawnRocket(m_launcher.position);

//		m_rigidbody.AddExplosionForce(120, m_launcher1.position, 3);
//		m_launcher.GetComponent<ParticleSystem>().Play();
//		m_rigidbody.AddTorque(0, 30, 0);
	}

	public override bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
		}
	}

}
