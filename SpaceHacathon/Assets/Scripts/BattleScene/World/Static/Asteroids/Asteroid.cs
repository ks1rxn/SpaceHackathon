using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Static.Asteroids {

	public class Asteroid : MonoBehaviour {
		private float _rotationSpeed;
		private Vector3 _rotationVector;

		public void Initiate(AsteroidGroupType type, Vector3 position, float size) {
			transform.localPosition = position;
			_rotationVector = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
			transform.Rotate(_rotationVector, MathHelper.Random.Next(360));

			switch (type) {
				case AsteroidGroupType.Large:
					transform.localScale = new Vector3(size, size, size);
					_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 0.25f;
					break;
				case AsteroidGroupType.Medium:
					transform.localScale = new Vector3(size, size, size);
					_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 0.5f;
					break;
				case AsteroidGroupType.Small:
					transform.localScale = new Vector3(size, size, size);
					_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 0.75f;
					break;
			}
		}

		public void UpdateRotation() {
			transform.Rotate(_rotationVector, _rotationSpeed);
		}

	}

}
