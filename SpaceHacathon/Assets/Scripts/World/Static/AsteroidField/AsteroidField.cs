using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AsteroidField : MonoBehaviour {
	[SerializeField]
	private float m_blockSize;
	[SerializeField]
	private int m_density;
	[SerializeField]
	private GameObject[] m_asteroidPrefabs;

	private List<GameObject> m_asteroids;

	Random m_random = new Random();

	protected void Awake() {
		m_asteroids = new List<GameObject>();
		CreateBlock();
	}

	private void FixedUpdate() { 
		ClearAsteroids();
		SpawnAsteroid();
	}

	private GameObject CreateBlock() {
		GameObject go = new GameObject("block");
		go.transform.parent = transform;
		for (int i = 0; i != m_density; i++) {
			GameObject asteroid = CreateNewAsteroid();
			asteroid.SetActive(true);
			float x = (float) m_random.NextDouble() * m_blockSize - m_blockSize / 2;
			float y = (float) m_random.NextDouble() * m_blockSize - m_blockSize / 2;
			asteroid.transform.position = new Vector3(x, -3, y);
			asteroid.transform.parent = go.transform;
		}
		return go;
	}

	private void ClearAsteroids() {
		foreach (GameObject asteroid in m_asteroids) {
			Vector3 projection = new Vector3(asteroid.transform.position.x, 0, asteroid.transform.position.z);
			if (Vector3.Distance(BattleContext.PlayerShip.transform.position, projection) > 10) {
				asteroid.SetActive(false);
			}
		}
	}

	private GameObject SpawnAsteroid() {
		return null;
	}

	private GameObject CreateNewAsteroid() {
		int index = m_random.Next(3);
		GameObject asteroid =  Instantiate(m_asteroidPrefabs[index]);
		return asteroid;
	}

	private int ActiveAsteroidsCount() {
		int count = 0;
		foreach (GameObject asteroid in m_asteroids) {
			if (asteroid.activeInHierarchy) {
				count++;
			}
		}
		return count;
	}

}
