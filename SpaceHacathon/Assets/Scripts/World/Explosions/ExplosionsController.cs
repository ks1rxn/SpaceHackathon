using System.Collections.Generic;
using UnityEngine;

public class ExplosionsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerShipExplosionPrefab;
	[SerializeField]
	private GameObject m_rocketExplosionPrefab;
	[SerializeField]
	private GameObject m_blasterShipExplosionPrefab;

	private List<Explosion> m_explosions; 

	protected void Awake() {
		BattleContext.ExplosionsController = this;

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

	private void SpawnExplosion(ExplosionType type, Vector3 position) {
		Explosion targetExplosion = null;
		foreach (Explosion explosion in m_explosions) {
			if (explosion.ExplosionType == type && !explosion.gameObject.activeInHierarchy) {
				targetExplosion = explosion;
				break;
			}
		}
		if (targetExplosion == null) {
			targetExplosion = CreateExplosion(type);
		}
		targetExplosion.Spawn(position);
	}

	private Explosion CreateExplosion(ExplosionType type) {
		switch (type) {
			case ExplosionType.PlayerShipExplosion:
				Explosion e1 = (Instantiate(m_playerShipExplosionPrefab)).GetComponent<Explosion>();
				e1.transform.parent = transform;
				e1.Initiate(ExplosionType.PlayerShipExplosion);
				m_explosions.Add(e1);
				return e1;
			case ExplosionType.RocketExplosion:
				Explosion e2 = (Instantiate(m_rocketExplosionPrefab)).GetComponent<Explosion>();
				e2.transform.parent = transform;
				e2.Initiate(ExplosionType.RocketExplosion);
				m_explosions.Add(e2);
				return e2;
			case ExplosionType.BlasterExplosion:
				Explosion e3 = (Instantiate(m_blasterShipExplosionPrefab)).GetComponent<Explosion>();
				e3.transform.parent = transform;
				e3.Initiate(ExplosionType.BlasterExplosion);
				m_explosions.Add(e3);
				return e3;
			default:
				return null;
		}
	}

}
