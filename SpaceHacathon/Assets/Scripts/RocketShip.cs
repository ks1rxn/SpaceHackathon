using System;
using UnityEngine;

public class RocketShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private Transform m_parent;

	[SerializeField]
	private GameObject m_rocketPrefab;

	private float m_gun1Cooldown;
	private float m_gun2Cooldown;
	private float m_globalCooldown;

	[SerializeField]
	private float m_gunCooldownValue;
	[SerializeField]
	private float m_globalCooldownValue;
	[SerializeField]
	private float m_rocketLifeTime;

	[SerializeField]
	private Transform m_launcher1;
	[SerializeField]
	private Transform m_launcher2;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
//		m_rigidbody.centerOfMass = new Vector3(1, 0, 0);
	}

	public void Spawn(Vector3 position, Transform parent) {
		m_parent = parent;
		transform.position = position;

		m_gun1Cooldown = 0;
		m_gun2Cooldown = 0;
		m_globalCooldown = m_globalCooldownValue;
	}

	protected void Update() {
		Vector3 enemyPosition = BattleContext.PlayerShip.transform.position;
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));

		if (m_rigidbody.velocity.magnitude > 0) {
			m_rigidbody.AddForce(-m_rigidbody.velocity * Time.deltaTime * 100);
		}

		float angle = MathHelper.AngleBetweenVectors(lookVector, enemyPosition - transform.position);
		if (Mathf.Abs(angle) > 10) {
			float angleSign = 0;
			if (!angle.Equals(0)) {
				angleSign = angle / Mathf.Abs(angle);
			}
			angleSign *= 10;
			m_rigidbody.AddTorque(new Vector3(0, (angleSign - m_rigidbody.angularVelocity.y * 50) * 5f * Time.deltaTime, 0));
			if (m_rigidbody.angularVelocity.magnitude > 1f) {
				m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 1f;
			}
		}

		if ((Mathf.Abs(angle) < 45) && Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) < 20) {
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

		m_gun1Cooldown -= Time.deltaTime;
		m_gun2Cooldown -= Time.deltaTime;
		m_globalCooldown -= Time.deltaTime;
	}

	private void SpawnRocket1() {
		Rocket rocket = ((GameObject)Instantiate(m_rocketPrefab)).GetComponent<Rocket>();
		rocket.transform.parent = m_parent;
		rocket.transform.rotation = transform.rotation;
		rocket.Spawn(m_launcher1.position, m_rocketLifeTime);
		m_rigidbody.AddExplosionForce(120, m_launcher1.position, 3);
		m_launcher1.GetComponent<ParticleSystem>().Play();
		m_rigidbody.AddTorque(0, 30, 0);
	}

	private void SpawnRocket2() {
//		Rocket rocket = ((GameObject)Instantiate(m_rocketPrefab)).GetComponent<Rocket>();
//		rocket.transform.parent = m_parent;
//		rocket.transform.rotation = transform.rotation;
//		rocket.Spawn(m_launcher2.position, m_rocketLifeTime);
//		m_rigidbody.AddExplosionForce(120, m_launcher2.position, 3);
//		m_launcher2.GetComponent<ParticleSystem>().Play();
//		m_rigidbody.AddTorque(0, -30, 0);
	}

}
