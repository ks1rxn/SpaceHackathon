using UnityEngine;

public class Asteroid : MonoBehaviour {
	private float m_rotationSpeed;
	private Vector3 m_rotationVector;

	public void Initiate(AsteroidGroupType type, Vector3 position, float size) {
		transform.localPosition = position;
		m_rotationVector = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
		transform.Rotate(m_rotationVector, MathHelper.Random.Next(360));

		switch (type) {
			case AsteroidGroupType.Large:
				transform.localScale = new Vector3(size, size, size);
				m_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 0.25f;
				break;
			case AsteroidGroupType.Medium:
				transform.localScale = new Vector3(size, size, size);
				m_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 0.5f;
				break;
			case AsteroidGroupType.Small:
				transform.localScale = new Vector3(size, size, size);
				m_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 0.75f;
				break;
		}
	}

	public void UpdateRotation() {
		transform.Rotate(m_rotationVector, m_rotationSpeed);
	}

}
