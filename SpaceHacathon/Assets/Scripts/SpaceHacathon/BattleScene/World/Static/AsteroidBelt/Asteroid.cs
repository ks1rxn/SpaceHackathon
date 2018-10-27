using UnityEngine;
using Zenject;
using Random = System.Random;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt {

	public class Asteroid : MonoBehaviour {
		private float _rotationSpeed;
		private Vector3 _rotationVector;
		private Random _random;

		[Inject]
		private void Construct(Random random) {
			_random = random;
		}
		
		public void Initiate(AsteroidGroupType type, Vector3 position, float size) {
			transform.localPosition = position;
			_rotationVector = new Vector3((float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f, (float)_random.NextDouble() - 0.5f).normalized;
			transform.Rotate(_rotationVector, _random.Next(360));
			transform.localScale = new Vector3(size, size, size);
			
			switch (type) {
				case AsteroidGroupType.Large:
					_rotationSpeed = ((float) _random.NextDouble() - 0.5f) * 0.25f;
					break;
				case AsteroidGroupType.Medium:
					_rotationSpeed = ((float) _random.NextDouble() - 0.5f) * 0.5f;
					break;
				case AsteroidGroupType.Small:
					_rotationSpeed = ((float) _random.NextDouble() - 0.5f) * 0.75f;
					break;
			}
		}

		public void UpdateRotation() {
			transform.Rotate(_rotationVector, _rotationSpeed);
		}

		public class Factory : PlaceholderFactory<string, Asteroid> { }

	}

}
