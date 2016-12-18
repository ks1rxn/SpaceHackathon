using System;
using System.Collections;
using UnityEngine;

public class RocketShip : MonoBehaviour, IEnemyShip {
	private Rigidbody m_rigidbody;

	private float m_gun1Cooldown;
	private float m_gun2Cooldown;
	private float m_globalCooldown;

	[SerializeField]
	private float m_gunCooldownValue;
	[SerializeField]
	private float m_globalCooldownValue;

	[SerializeField]
	private Transform m_launcher1;
	[SerializeField]
	private Transform m_launcher2;

	[SerializeField]
	private ParticleSystem m_spawnEffect;
	private Material m_material;

	[SerializeField]
	private GameObject m_chargeTarget;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
		m_material = GetComponent<MeshRenderer>().material;
	}

	public void Spawn(Vector3 position, float angle) {
		transform.position = position;

		m_gun1Cooldown = 0;
		m_gun2Cooldown = 0;
		m_globalCooldown = m_globalCooldownValue;

		StartCoroutine(SpawnEffect());

		UncheckAsTarget();
	}

	private IEnumerator SpawnEffect() {
		m_spawnEffect.Play();
		float value = 1.0f;
		while (value > 0) {
			value -= Time.deltaTime;
			m_material.SetFloat("_SliceAmount", value);
			yield return new WaitForEndOfFrame();
		}
		m_material.SetFloat("_SliceAmount", 0.0f);
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

	public void CheckAsTarget() {
		m_chargeTarget.SetActive(true);
	}

	public void UncheckAsTarget() {
		m_chargeTarget.SetActive(false);
	}

	public void UpdateShip() {
		Vector3 enemyPosition = BattleContext.PlayerShip.Position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		if (m_rigidbody.velocity.magnitude > 0) {
			m_rigidbody.AddForce(-m_rigidbody.velocity * 2);
		}

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		if (Mathf.Abs(angle) > 10) {
			float angleSign = 0;
			if (!angle.Equals(0)) {
				angleSign = angle / Mathf.Abs(angle);
			}
			angleSign *= 10;
			m_rigidbody.AddTorque(new Vector3(0, (angleSign - m_rigidbody.angularVelocity.y * 50) * 0.1f, 0));
			if (m_rigidbody.angularVelocity.magnitude > 1f) {
				m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 1f;
			}
		}

		if ((Mathf.Abs(angle) < 45) && Vector3.Distance(BattleContext.PlayerShip.Position, transform.position) < 20) {
			if ((m_gun1Cooldown <= 0) && (m_globalCooldown <= 0)) {
				SpawnRocket1();
				m_gun1Cooldown = m_gunCooldownValue;
				m_globalCooldown = m_globalCooldownValue;
			} else if ((m_gun2Cooldown <= 0) && (m_globalCooldown <= 0)) {
				SpawnRocket2();
				m_gun2Cooldown = m_gunCooldownValue;
				m_globalCooldown = m_globalCooldownValue;
			}
		}

		m_gun1Cooldown -= 10 * Time.fixedDeltaTime;
		m_gun2Cooldown -= 10 * Time.fixedDeltaTime;
		m_globalCooldown -= 10 * Time.fixedDeltaTime;

		if (Vector3.Distance(BattleContext.PlayerShip.Position, transform.position) > 80) {
			Die();
		}
	}

	private void SpawnRocket1() {
		BattleContext.BulletsController.SpawnRocket(m_launcher1.position, transform.rotation.eulerAngles.y);

		m_rigidbody.AddExplosionForce(120, m_launcher1.position, 3);
		m_launcher1.GetComponent<ParticleSystem>().Play();
		m_rigidbody.AddTorque(0, 30, 0);
	}

	private void SpawnRocket2() {
		BattleContext.BulletsController.SpawnRocket(m_launcher2.position, transform.rotation.eulerAngles.y);

		m_rigidbody.AddExplosionForce(120, m_launcher2.position, 3);
		m_launcher2.GetComponent<ParticleSystem>().Play();
		m_rigidbody.AddTorque(0, -30, 0);
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}
