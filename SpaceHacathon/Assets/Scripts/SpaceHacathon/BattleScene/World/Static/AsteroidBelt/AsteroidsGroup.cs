using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt {

	public class AsteroidsGroup : MonoBehaviour {
		private Random _random;
		private Asteroid.Factory _asteroidFactory;
		
		private Vector3 _rotationVector;
		private float _rotationSpeed;

		private List<Asteroid> _asteroids;

		[Inject]
		private void Construct(Asteroid.Factory asteroidFactory, Random random) {
			_asteroidFactory = asteroidFactory;
			_random = random;
		}
		
		public void Initiate(AsteroidGroupType type, Vector3 position) {
			transform.localPosition = position;

			_asteroids = new List<Asteroid>();
			_rotationVector = new Vector3((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f).normalized;
			transform.Rotate(_rotationVector, _random.Next(360));

			_rotationSpeed = ((float) _random.NextDouble() - 0.5f) * 0.4f;
			_rotationSpeed += 0.15f * Mathf.Sign(_rotationSpeed);
			switch (type) {
				case AsteroidGroupType.Large:
					SpawnAsLargeGroup();
					break;
				case AsteroidGroupType.Medium:
					SpawnAsMediumGroup();
					break;
				case AsteroidGroupType.Small:
					SpawnAsSmallGroup();
					break;
			}
		}

		public void UpdateRotations() {
			transform.Rotate(_rotationVector, _rotationSpeed);
			for (int i = 0; i != _asteroids.Count; i++) { 
				_asteroids[i].UpdateRotation();
			}
		}

		private void SpawnAsLargeGroup() {
			Asteroid asteroid = CreateNewAsteroid();
			float sizeBig = 3.2f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 100;
			asteroid.Initiate(AsteroidGroupType.Large, new Vector3(0, 0, 0), sizeBig);

			if (_random.Next(100) < 30) {
				asteroid = CreateNewAsteroid();
				float sizeSmall = 1f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 30;
				Vector3 direction = new Vector3((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f).normalized;
				float distance = sizeBig / 2 + sizeSmall / 1.5f;
				asteroid.Initiate(AsteroidGroupType.Small, direction * distance, sizeSmall);
			}
		}

		private void SpawnAsMediumGroup() {
			Asteroid asteroid = CreateNewAsteroid();
			float size1 = 1.7f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 30;
			float size2 = 1.7f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 30;
			float distance = (size1 / 2 + size2 / 2) * ((float)_random.NextDouble() * 3 + 1);
			Vector3 asteroidDirection = new Vector3((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f).normalized;
			asteroid.Initiate(AsteroidGroupType.Medium, asteroidDirection * distance / 2, size1);
			asteroid = CreateNewAsteroid();
			asteroid.Initiate(AsteroidGroupType.Medium, -asteroidDirection * distance / 2, size2);
		}

		private void SpawnAsSmallGroup() {
			float sizeBig = 1.7f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 30;
			Asteroid asteroid = CreateNewAsteroid();
			asteroid.Initiate(AsteroidGroupType.Medium, Vector3.zero, sizeBig);

			float size1 = 0.85f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 30;
			float distance1 = (sizeBig / 2 + size1 / 2) * ((float)_random.NextDouble() * 2 + 1);
			Vector3 asteroidDirection1 = new Vector3((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f).normalized;
			asteroid = CreateNewAsteroid();
			asteroid.Initiate(AsteroidGroupType.Medium, asteroidDirection1 * distance1, size1);

			while (true) {
				float size2 = 1f + Mathf.Pow((float) _random.NextDouble() - 0.5f, 7) * 30;
				float distance2 = sizeBig / 2 + size2 / 2;
				Vector3 asteroidDirection2 = new Vector3((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f).normalized;
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
			int index = _random.Next(3) + 1;
			Asteroid asteroid = _asteroidFactory.Create($"Prefabs/Asteroids/Asteroid{index}");
			asteroid.transform.parent = transform;
			_asteroids.Add(asteroid);
			return asteroid;
		}

		public class Factory : PlaceholderFactory<string, AsteroidsGroup> { }

	}

}