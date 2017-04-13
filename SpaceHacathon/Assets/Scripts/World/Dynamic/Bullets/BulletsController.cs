using System.Collections.Generic;
using UnityEngine;

public class BulletsController : IController {
	private SettingsBulletsController m_settings;

	private List<StunProjectile> m_stunProjectiles;
	private List<Laser> m_lasers;
	private List<Missile> m_missiles; 
	private List<CarrierRocket> m_carrierRockets; 

	public override void Initiate() {
		m_settings = BattleContext.Settings.BulletsController;

		m_stunProjectiles = new List<StunProjectile>();
		m_lasers = new List<Laser>();
		m_missiles = new List<Missile>();
		m_carrierRockets = new List<CarrierRocket>();

		for (int i = 0; i != m_settings.MissilesLimit; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.Missile, gameObject, m_missiles);
		}
		for (int i = 0; i != 5; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.StunProjectile, gameObject, m_stunProjectiles);
		}
		for (int i = 0; i != 10; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.Laser, gameObject, m_lasers);
		}
		for (int i = 0; i != 3; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.CarrierRocket, gameObject, m_carrierRockets);
		}
	}

	public override void FixedUpdateEntity() {
		for (int i = 0; i != m_missiles.Count; i++) {
			if (m_missiles[i].IsSpawned()) {
				m_missiles[i].FixedUpdateEntity();
			}
		}
		for (int i = 0; i != m_stunProjectiles.Count; i++) {
			if (m_stunProjectiles[i].IsSpawned()) {
				m_stunProjectiles[i].FixedUpdateEntity();
			}
		}
		for (int i = 0; i != m_lasers.Count; i++) {
			if (m_lasers[i].IsSpawned()) {
				m_lasers[i].FixedUpdateEntity();
			}
		}
		for (int i = 0; i != m_carrierRockets.Count; i++) {
			if (m_carrierRockets[i].IsSpawned()) {
				m_carrierRockets[i].FixedUpdateEntity();
			}
		}
	}

	public void SpawnMissile(Vector3 position, float angle) {
		int aliveMissiles = 0;
		foreach (Missile rocket in m_missiles) {
			if (rocket.IsSpawned()) {
				aliveMissiles++;
			}
		}
		if (aliveMissiles >= m_settings.MissilesLimit) {
			return;
		}
		EntitiesHelper.SpawnEntity(AvailablePrefabs.Missile, gameObject, m_missiles, position, angle);
	}

	public void SpawnCarrierRocket(Vector3 position) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.CarrierRocket, gameObject, m_carrierRockets, position, 0);
	}

	public void SpawnStunProjectile(Vector3 position, float angle) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.StunProjectile, gameObject, m_stunProjectiles, position, angle);
	}

	public void SpawnLaser(Vector3 position, float angle) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.Laser, gameObject, m_lasers, position, angle);
	}

}
