using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BulletsController : MonoBehaviour {
	[SerializeField]
	private int m_missilesLimit;

	[SerializeField]
	private GameObject m_stunProjectilePrefab;
	[SerializeField]
	private GameObject m_laserPrefab;
	[SerializeField]
	private GameObject m_missilePrefab;
	[SerializeField]
	private GameObject m_droneCarrierRocket;

	private List<StunProjectile> m_stunProjectiles;
	private List<Laser> m_lasers;
	private List<Missile> m_missiles; 
	private List<CarrierRocket> m_carrierRockets; 

	public void Initiate() {
		m_stunProjectiles = new List<StunProjectile>();
		m_lasers = new List<Laser>();
		m_missiles = new List<Missile>();
		m_carrierRockets = new List<CarrierRocket>();

		for (int i = 0; i != m_missilesLimit; i++) {
			CreateMissile();
		}
		for (int i = 0; i != 5; i++) {
			CreateStunProjectile();
		}
		for (int i = 0; i != 10; i++) {
			CreateLaser();
		}
		for (int i = 0; i != 3; i++) {
			CreateCarrierRocket();
		}
	}

	public void UpdateEntity() {
		for (int i = 0; i != m_missiles.Count; i++) {
			if (m_missiles[i].IsAlive) {
				m_missiles[i].UpdateBullet();
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
		for (int i = 0; i != m_carrierRockets.Count; i++) {
			if (m_carrierRockets[i].IsAlive) {
				m_carrierRockets[i].UpdateBullet();
			}
		}
	}

	public Missile SpawnMissile(Vector3 position, float angle) {
		Missile targetRocket = null;
		int aliveMissiles = 0;
		foreach (Missile rocket in m_missiles) {
			if (!rocket.IsAlive) {
				targetRocket = rocket;
			} else {
				aliveMissiles++;
			}
		}
		if (aliveMissiles >= m_missilesLimit) {
			return null;
		}
		if (targetRocket == null) {
			targetRocket = CreateMissile();
		}
		targetRocket.Spawn(position, angle);
		return targetRocket;
	}

	public CarrierRocket SpawnCarrierRocket(Vector3 position) {
		CarrierRocket targetRocket = null;
		foreach (CarrierRocket rocket in m_carrierRockets) {
			if (!rocket.IsAlive) {
				targetRocket = rocket;
				break;
			}
		}
		if (targetRocket == null) {
			targetRocket = CreateCarrierRocket();
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

	private Missile CreateMissile() {
		Missile missile = (Instantiate(m_missilePrefab)).GetComponent<Missile>();
		missile.transform.parent = transform;
		missile.Initiate();
		m_missiles.Add(missile);
		return missile;
	}

	private CarrierRocket CreateCarrierRocket() {
		CarrierRocket missile = (Instantiate(m_droneCarrierRocket)).GetComponent<CarrierRocket>();
		missile.transform.parent = transform;
		missile.Initiate();
		m_carrierRockets.Add(missile);
		return missile;
	}

}
