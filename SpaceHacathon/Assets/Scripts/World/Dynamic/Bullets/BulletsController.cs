using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BulletsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketPrefab;
	[SerializeField]
	private GameObject m_blasterPrefab;
	[SerializeField]
	private GameObject m_laserPrefab;

	private List<Rocket> m_rockets;
	private List<Blaster> m_blasters;
	private List<Laser> m_lasers;

	private Random m_random = new Random();

	private void Awake() {
		BattleContext.BulletsController = this;

		m_rockets = new List<Rocket>();
		m_blasters = new List<Blaster>();
		m_lasers = new List<Laser>();

		for (int i = 0; i != 10; i++) {
			CreateRocket();
		}
		for (int i = 0; i != 5; i++) {
			CreateBlaster();
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
		for (int i = 0; i != m_blasters.Count; i++) {
			if (m_blasters[i].IsAlive) {
				m_blasters[i].UpdateBullet();
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

	public Blaster SpawnBlaster(Vector3 position, float angle) {
		Blaster targetBlaster = null;
		foreach (Blaster blaster in m_blasters) {
			if (!blaster.IsAlive) {
				targetBlaster = blaster;
				break;
			}
		}
		if (targetBlaster == null) {
			targetBlaster = CreateBlaster();
		}
		targetBlaster.Spawn(position, angle);
		return targetBlaster;
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

	private Blaster CreateBlaster() {
		Blaster blaster = (Instantiate(m_blasterPrefab)).GetComponent<Blaster>();
		blaster.transform.parent = transform;
		blaster.Initiate();
		m_blasters.Add(blaster);
		return blaster;
	}

	private Laser CreateLaser() {
		Laser laser = (Instantiate(m_laserPrefab)).GetComponent<Laser>();
		laser.transform.parent = transform;
		laser.Initiate();
		m_lasers.Add(laser);
		return laser;
	}

}
