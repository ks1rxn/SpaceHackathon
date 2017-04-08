using System.Collections.Generic;
using UnityEngine;

public class ExplosionsController : IController {
	private List<IExplosion> m_explosions;
	
	private List<ShipExplosion> m_shipExplosions; 
	private List<RocketExplosion> m_rocketExplosions; 
	private List<MineExplosion> m_mineExplosions; 
	private List<LaserExplosion> m_laserExplosions; 

	public override void Initiate() {
		m_explosions = new List<IExplosion>();

		m_shipExplosions = new List<ShipExplosion>();
		m_rocketExplosions = new List<RocketExplosion>();
		m_mineExplosions = new List<MineExplosion>();
		m_laserExplosions = new List<LaserExplosion>();

		for (int i = 0; i != 4; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.ShipExplosion, gameObject, m_shipExplosions, m_explosions);
		}

		for (int i = 0; i != 4; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.LaserExplosion, gameObject, m_laserExplosions, m_explosions);
		}

		for (int i = 0; i != 5; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.RocketExplosion, gameObject, m_rocketExplosions, m_explosions);
		}

		for (int i = 0; i != 3; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.MineExplosion, gameObject, m_mineExplosions, m_explosions);
		}
	}

	public override void FixedUpdateEntity() {
		foreach (IExplosion explosion in m_explosions) {
			if (explosion.IsSpawned()) {
				explosion.FixedUpdateEntity();
			}
		}
	}

	public void ShipExplosion(Vector3 position) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.ShipExplosion, gameObject, m_shipExplosions, m_explosions, position, 0);
	}

	public void RocketExplosion(Vector3 position) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.RocketExplosion, gameObject, m_rocketExplosions, m_explosions, position, 0);
	}

	public void LaserExplosion(Vector3 position) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.LaserExplosion, gameObject, m_laserExplosions, m_explosions, position, 0);
	}

	public void MineExplosion(Vector3 position) {
		EntitiesHelper.SpawnEntity(AvailablePrefabs.MineExplosion, gameObject, m_mineExplosions, m_explosions, position, 0);
	}

}
