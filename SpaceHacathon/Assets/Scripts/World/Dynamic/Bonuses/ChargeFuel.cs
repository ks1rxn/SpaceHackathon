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

	public void UpdateState() {
		transform.Rotate(m_rotationVector, m_rotationSpeed);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<PlayerShip>() != null) {
			Die();
		} else {
			Die();
		}
	}

	public void Die() {
		BattleContext.BonusesController.Respawn(this);
	}

}
