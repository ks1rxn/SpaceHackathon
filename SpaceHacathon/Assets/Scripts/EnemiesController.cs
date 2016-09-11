using UnityEngine;

public class EnemiesController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketShipPrefab;

	protected void Awake() {
		SpawnRocketShip(new Vector3(5, 0, 5));
	}

	protected void Update() {

	}

	private void SpawnRocketShip(Vector3 position) {
		RocketShip rocketShip = ((GameObject)Instantiate(m_rocketShipPrefab)).GetComponent<RocketShip>();
		rocketShip.transform.parent = transform;
		rocketShip.Spawn(position);
	}

}
