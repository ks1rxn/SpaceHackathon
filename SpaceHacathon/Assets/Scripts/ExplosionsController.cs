using UnityEngine;

public class ExplosionsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerShipExplosionPrefab;
	[SerializeField]
	private GameObject m_rocketExplosionPrefab;
	[SerializeField]
	private GameObject m_blasterShipExplosionPrefab;

	protected void Awake() {
		BattleContext.ExplosionsController = this;
	}

	public void PlayerShipExplosion(Vector3 position) {
		Explosion explosion = ((GameObject)Instantiate(m_playerShipExplosionPrefab)).GetComponent<Explosion>();
		explosion.transform.parent = gameObject.transform;
		explosion.Spawn(position);
	}

	public void RocketExplosion(Vector3 position) {
		Explosion explosion = ((GameObject)Instantiate(m_rocketExplosionPrefab)).GetComponent<Explosion>();
		explosion.transform.parent = gameObject.transform;
		explosion.Spawn(position);
	}

	public void BlasterExplosion(Vector3 position) {
		Explosion explosion = ((GameObject)Instantiate(m_blasterShipExplosionPrefab)).GetComponent<Explosion>();
		explosion.transform.parent = gameObject.transform;
		explosion.Spawn(position);
	}

}
