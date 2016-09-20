using UnityEngine;

public class ExplosionsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerShipExplosionPrefab;

	protected void Awake() {
		BattleContext.ExplosionsController = this;
	}

	public void PlayerShipExplosion(Vector3 position) {
		Explosion explosion = ((GameObject)Instantiate(m_playerShipExplosionPrefab)).GetComponent<Explosion>();
		explosion.transform.parent = gameObject.transform;
		explosion.Spawn(position);
	}

}
