using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BulletsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketPrefab;
	[SerializeField]
	private GameObject m_stunProjectilePrefab;
	[SerializeField]
	private GameObject m_laserPrefab;

	private List<Rocket> m_rockets;
	private List<StunProjectile> m_stunProjectiles;
	private List<Laser> m_lasers;

	private Random m_random = new Random();

	private void Awake() {
		BattleContext.BulletsController = this;

		m_rockets = new List<Rocket>();
		m_stunProjectiles = new List<StunProjectile>();
		m_lasers = new List<Laser>();

		for (int i = 0; i != 10; i++) {
			CreateRocket();
		}
		for (int i = 0; i != 5; i++) {
			CreateStunProjectile();
		}
		for (int i = 0; i != 10; i++) {
			CreateLaser();
		}
	}

	private void FixedUpdate() {
		for (int i = 0; i != m_rockets.Count; i++) {
			if (m_rockets[i].IsAlive) {
				m_rockets[i].UpdateBullet();
			}
		}
		for (int i = 0; i != m_stunProjectiles.Count; i++) {
			if (m_stunProjectiles[i].IsAlive) {
				m_stunProjectiles[i].UpdateBullet();
			}
		}
		for (int i = 0; i != m_lasers.Count; i++) {
			if (m_lasers[i].IsAlive) {
				m_lasers[i].UpdateBullet();
			}
		}
	}

	public Rocket SpawnRocket(Vector3 position) {
		Rocket targetRocket = null;
		foreach (Rocket rocket in m_rockets) {
			if (!rocket.IsAlive) {
				targetRocket = rocket;
				break;
			}
		}
		if (targetRocket == null) {
			targetRocket = CreateRocket();
		}
		targetRocket.Spawn(position);
		return targetRocket;
	}

	public StunProjectile SpawnStunProjectile(Vector3 position, float angle) {
		StunProjectile targetStunProjectile = null;
		foreach (StunProjectile blaster in m_stunProjectiles) {
			if (!blaster.IsAlive) {
				targetStunProjectile = blaster;
				break;
			}
		}
		if (targetStunProjectile == null) {
			targetStunProjectile = CreateStunProjectile();
		}
		targetStunProjectile.Spawn(position, angle);
		return targetStunProjectile;
	}

	public Laser SpawnLaser(Vector3 position, float angle) {
		Laser targetLaser = null;
		foreach (Laser laser in m_lasers) {
			if (!laser.IsAlive) {
				targetLaser = laser;
				break;
			}
		}
		if (targetLaser == null) {
			targetLaser = CreateLaser();
		}
		targetLaser.Spawn(position, angle);
		return targetLaser;
	}

	private Rocket CreateRocket() {
		Rocket rocket = (Instantiate(m_rocketPrefab)).GetComponent<Rocket>();
		rocket.transform.parent = transform;
		rocket.Initiate();
		m_rockets.Add(rocket);
		return rocket;
	}

	private StunProjectile CreateStunProjectile() {
		StunProjectile stunProjectile = (Instantiate(m_stunProjectilePrefab)).GetComponent<StunProjectile>();
		stunProjectile.transform.parent = transform;
		stunProjectile.Initiate();
		m_stunProjectiles.Add(stunProjectile);
		return stunProjectile;
	}

	private Laser CreateLaser() {
		Laser laser = (Instantiate(m_laserPrefab)).GetComponent<Laser>();
		laser.transform.parent = transform;
		laser.Initiate();
		m_lasers.Add(laser);
		return laser;
	}

}
