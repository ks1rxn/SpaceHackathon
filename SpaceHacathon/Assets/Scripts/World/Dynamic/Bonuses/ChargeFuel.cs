using UnityEngine;

public class ChargeFuel : MonoBehaviour {
	private Vector3 m_rotationVector;
	private float m_rotationSpeed;

	public void Spawn(Vector3 position) {
		transform.position = position;
		Vector3 intialRotation = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
		transform.Rotate(intialRotation, MathHelper.Random.Next(360));

		m_rotationVector = new Vector3(0, 1, 0);
		m_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 2f;
	}

	private void FixedUpdate() {
		if (Vector3.Distance(Position, BattleContext.PlayerShip.transform.position) < 50) {
			transform.Rotate(m_rotationVector, m_rotationSpeed);
		}
		if (Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) > 80) {
			Die();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<PlayerShip>() != null) {
			Die();
		} else {
			Die();
		}
	}

	private void Die() {
		BattleContext.BonusesController.Respawn(this);
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}
