using System.Collections.Generic;
using UnityEngine;

public class ExplosionsController : IController {
	[SerializeField]
	private GameObject m_playerShipExplosionPrefab;
	[SerializeField]
	private GameObject m_rocketExplosionPrefab;
	[SerializeField]
	private GameObject m_blasterShipExplosionPrefab;
	[SerializeField]
	private GameObject m_mineExplosionPrefab;

	private List<Explosion> m_explosions; 

	public override void Initiate() {
		m_explosions = new List<Explosion>();

		CreateExplosion(ExplosionType.PlayerShipExplosion);
		CreateExplosion(ExplosionType.PlayerShipExplosion);
		CreateExplosion(ExplosionType.PlayerShipExplosion);
		CreateExplosion(ExplosionType.PlayerShipExplosion);

		CreateExplosion(ExplosionType.BlasterExplosion);
		CreateExplosion(ExplosionType.BlasterExplosion);
		CreateExplosion(ExplosionType.BlasterExplosion);
		CreateExplosion(ExplosionType.BlasterExplosion);

		CreateExplosion(ExplosionType.RocketExplosion);
		CreateExplosion(ExplosionType.RocketExplosion);
		CreateExplosion(ExplosionType.RocketExplosion);
		CreateExplosion(ExplosionType.RocketExplosion);
		CreateExplosion(ExplosionType.RocketExplosion);

		CreateExplosion(ExplosionType.MineExplosion);
		CreateExplosion(ExplosionType.MineExplosion);
		CreateExplosion(ExplosionType.MineExplosion);
	}

	public override void FixedUpdateEntity() {
		foreach (Explosion explosion in m_explosions) {
			if (explosion.IsSpawned()) {
				explosion.FixedUpdateEntity();
			}
		}
	}

	public void PlayerShipExplosion(Vector3 position) {
		SpawnExplosion(ExplosionType.PlayerShipExplosion, position);
	}

	public void RocketExplosion(Vector3 position) {
		SpawnExplosion(ExplosionType.RocketExplosion, position);
	}

	public void BlasterExplosion(Vector3 position) {
		SpawnExplosion(ExplosionType.BlasterExplosion, position);
	}

	public void MineExplosion(Vector3 position) {
		SpawnExplosion(ExplosionType.MineExplosion, position);
	}

	private void SpawnExplosion(ExplosionType type, Vector3 position) {
		if (Vector3.Distance(BattleContext.Director.PlayerPosition, position) > 30) {
			return;
		}
		Explosion targetExplosion = null;
		foreach (Explosion explosion in m_explosions) {
			if (explosion.ExplosionType == type && !explosion.IsSpawned()) {
				targetExplosion = explosion;
				break;
			}
		}
		if (targetExplosion == null) {
			targetExplosion = CreateExplosion(type);
		}
		targetExplosion.Spawn(position, 0);
	}

	private Explosion CreateExplosion(ExplosionType type) {
		switch (type) {
			case ExplosionType.PlayerShipExplosion:
				Explosion e1 = (Instantiate(m_playerShipExplosionPrefab)).GetComponent<Explosion>();
				e1.gameObject.SetActive(false);
				e1.transform.parent = transform;
				e1.Initiate();
				m_explosions.Add(e1);
				return e1;
			case ExplosionType.RocketExplosion:
				Explosion e2 = (Instantiate(m_rocketExplosionPrefab)).GetComponent<Explosion>();
				e2.gameObject.SetActive(false);
				e2.transform.parent = transform;
				e2.Initiate();
				m_explosions.Add(e2);
				return e2;
			case ExplosionType.BlasterExplosion:
				Explosion e3 = (Instantiate(m_blasterShipExplosionPrefab)).GetComponent<Explosion>();
				e3.gameObject.SetActive(false);
				e3.transform.parent = transform;
				e3.Initiate();
				m_explosions.Add(e3);
				return e3;
			case ExplosionType.MineExplosion:
				Explosion e4 = (Instantiate(m_mineExplosionPrefab)).GetComponent<Explosion>();
				e4.gameObject.SetActive(false);
				e4.transform.parent = transform;
				e4.Initiate();
				m_explosions.Add(e4);
				return e4;
			default:
				return null;
		}
	}

}
