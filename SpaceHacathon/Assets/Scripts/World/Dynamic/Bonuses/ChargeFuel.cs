using UnityEngine;

public class ChargeFuel : MonoBehaviour {
	private Vector3 m_rotationVector;
	private float m_rotationSpeed;

	[SerializeField]
	private CollisionDetector m_collisionDetector;

	public void Initiate() {
		m_collisionDetector.Initiate();
		m_collisionDetector.RegisterDefaultListener(OnTargetHit);

		IsAlive = false;
	}

	public void Spawn(Vector3 position) {
		IsAlive = true;

		transform.position = position;
		Vector3 intialRotation = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
		transform.Rotate(intialRotation, MathHelper.Random.Next(360));

		m_rotationVector = new Vector3(0, 1, 0);
		m_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 2f;
	}

	public void UpdateState() {
		transform.Rotate(m_rotationVector, m_rotationSpeed);
	}

	private void OnTargetHit(GameObject other) {
		IsAlive = false;
	}

	public bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
		}
	}

}
