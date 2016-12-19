using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BulletsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_rocketPrefab;
	[SerializeField]
	private GameObject m_blasterPrefab;

	private List<Rocket> m_rockets;
	private List<Blaster> m_blasters;

	private Random m_random = new Random();

	private void Awake() {
		BattleContext.BulletsController = this;

		m_rockets = new List<Rocket>();
		m_blasters = new List<Blaster>();

		for (int i = 0; i != 10; i++) {
			CreateRocket();
		}
		for (int i = 0; i != 5; i++) {
			CreateBlaster();
		}
	}

	private void FixedUpdate() {
		for (int i = 0; i != m_rockets.Count; i++) {
			if (m_rockets[i].gameObject.activeInHierarchy) {
				m_rockets[i].UpdateBullet();
			}
		}
		for (int i = 0; i != m_blasters.Count; i++) {
			if (m_blasters[i].gameObject.activeInHierarchy) {
				m_blasters[i].UpdateBullet();
			}
		}
	}

	public void SpawnRocket(Vector3 position) {
		Rocket targetRocket = null;
		foreach (Rocket rocket in m_rockets) {
			if (!rocket.gameObject.activeInHierarchy) {
				targetRocket = rocket;
				break;
			}
		}
		if (targetRocket == null) {
			targetRocket = CreateRocket();
		}
		targetRocket.Spawn(position);
	}

	public void SpawnBlaster(Vector3 position, float angle) {
		Blaster targetBlaster = null;
		foreach (Blaster blaster in m_blasters) {
			if (!blaster.gameObject.activeInHierarchy) {
				targetBlaster = blaster;
				break;
			}
		}
		if (targetBlaster == null) {
			targetBlaster = CreateBlaster();
		}
		targetBlaster.Spawn(position, angle);
	}

	private Rocket CreateRocket() {
		Rocket rocket = (Instantiate(m_rocketPrefab)).GetComponent<Rocket>();
		rocket.gameObject.SetActive(false);
		rocket.transform.parent = transform;
		m_rockets.Add(rocket);
		return rocket;
	}

	private Blaster CreateBlaster() {
		Blaster blaster = (Instantiate(m_blasterPrefab)).GetComponent<Blaster>();
		blaster.gameObject.SetActive(false);
		blaster.transform.parent = transform;
		m_blasters.Add(blaster);
		return blaster;
	}

}
