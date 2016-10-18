using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AsteroidFieldBlockGroup : MonoBehaviour {
	private Random m_random = new Random();

	[SerializeField]
	private GameObject[] m_asteroidPrefabs;

	public void Initiate(AsteroidGroupType type, Vector3 position) {
		transform.localPosition = position;
		switch (type) {
			case AsteroidGroupType.Large:
				SpawnLargeGroup();
				break;
			case AsteroidGroupType.Medium:
				SpawnMediumGroup();
				break;
			case AsteroidGroupType.Small:
				SpawnSmallGroup();
				break;
		}
	}

	private void SpawnLargeGroup() {
		Asteroid asteroid = CreateNewAsteroid();
		float sizeBig = 3.2f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 100;
		asteroid.Initiate(AsteroidGroupType.Large, new Vector3(0, 0, 0), sizeBig);

		if (MathHelper.Random.Next(100) < 30) {
			asteroid = CreateNewAsteroid();
			float sizeSmall = 1f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 30;
			Vector3 direction = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
			float distance = sizeBig / 2 + sizeSmall / 1.5f;
			asteroid.Initiate(AsteroidGroupType.Small, direction * distance, sizeSmall);
		}
	}

	private void SpawnMediumGroup() {
		Asteroid asteroid = CreateNewAsteroid();
		float size1 = 1.7f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 30;
		float size2 = 1.7f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 30;
		float distance = size1 / 2 + size2 / 2;
		Vector3 asteroidDirection = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
		asteroid.Initiate(AsteroidGroupType.Medium, asteroidDirection * distance / 2, size1);
		asteroid = CreateNewAsteroid();
		asteroid.Initiate(AsteroidGroupType.Medium, -asteroidDirection * distance / 2, size2);
	}

	private void SpawnSmallGroup() {
		float sizeBig = 1.7f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 30;
		Asteroid asteroid = CreateNewAsteroid();
		asteroid.Initiate(AsteroidGroupType.Medium, Vector3.zero, sizeBig);

		float size1 = 1f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 30;
		float distance1 = sizeBig / 2 + size1 / 2;
		Vector3 asteroidDirection1 = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
		asteroid = CreateNewAsteroid();
		asteroid.Initiate(AsteroidGroupType.Medium, asteroidDirection1 * distance1, size1);

		while (true) {
			float size2 = 1f + Mathf.Pow((float) MathHelper.Random.NextDouble() - 0.5f, 7) * 30;
			float distance2 = sizeBig / 2 + size2 / 2;
			Vector3 asteroidDirection2 = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
			float length = Vector3.Distance(asteroidDirection2 * distance2, asteroidDirection1 * distance1);
			if (length < size1 / 2 + size2 / 2) {
				continue;
			}
			asteroid = CreateNewAsteroid();
			asteroid.Initiate(AsteroidGroupType.Medium, asteroidDirection2 * distance2, size2);
			break;
		}
		
	}

	private Asteroid CreateNewAsteroid() {
		int index = m_random.Next(m_asteroidPrefabs.Length);
		GameObject go =  Instantiate(m_asteroidPrefabs[index]);
		go.transform.parent = transform;
		Asteroid asteroid = go.GetComponent<Asteroid>();
		return asteroid;
	}

}

public enum AsteroidGroupType {
	Small = 0,
	Medium = 1,
	Large = 2
}
