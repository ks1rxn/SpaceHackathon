using UnityEngine;

public class RamShip : MonoBehaviour, IEnemyShip {
	private Rigidbody m_rigidbody;
	[SerializeField]
	private Transform m_hull;
	private RamShipState m_state;
	private readonly VectorPid headingController = new VectorPid(1.244681f, 0.1f, 1.1f);
	private float m_rotationSpeed;
	private float m_needRotationSpeed;

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, float angle) {
		transform.position = position;
		m_rotationSpeed = 0;
		m_needRotationSpeed = 0;

		m_state = RamShipState.Aiming;
	}

	public void Kill() {
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
		Die();
	}

	private void Die() {
		BattleContext.EnemiesController.Respawn(this);
	}

	private void OnTriggerEnter(Collider other) { 
		if (other.GetComponent<PlayerShip>() != null) {
			Kill();
		}
    }

	public void UpdateShip() {
		switch (m_state) {
			case RamShipState.Aiming:
				Aiming();
				break;
			case RamShipState.Stabilizing:
				Stabilizing();
				break;
			case RamShipState.Running:
				Running();
				break;
			case RamShipState.Stopping:
				Stopping();
				break;
		}

		if (m_rotationSpeed < m_needRotationSpeed) {
			m_rotationSpeed += 180 * Time.fixedDeltaTime;
		} else if (m_rotationSpeed > m_needRotationSpeed) {
			m_rotationSpeed -= 180 * Time.fixedDeltaTime;
		}
		m_hull.Rotate(m_rotationSpeed * Time.fixedDeltaTime, 0, 0);

		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) > 80) {
			Die();
		}
	}

	private void Aiming() {
		m_needRotationSpeed = 90;
		if (m_rigidbody.velocity.magnitude > 0.1f) {
			m_rigidbody.velocity *= 0.5f;
		}
		Vector3 headingCorrection = headingController.Update(Vector3.Cross(transform.right, BattleContext.PlayerShip.Position - Position), Time.fixedDeltaTime);
		m_rigidbody.AddTorque(headingCorrection * 0.2f);
		if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.PlayerShip.Position - Position)) < 5 &&
			m_rigidbody.angularVelocity.magnitude < 0.3f) {

			m_state = RamShipState.Running;
		}
	}

	private void Stabilizing() {
		m_needRotationSpeed = 50;
		if (m_rigidbody.angularVelocity.magnitude > 0.1f) {
			m_rigidbody.angularVelocity *= 0.5f;
		} else {
			m_state = RamShipState.Running;
		}
	}

	private void Running() {
		m_needRotationSpeed = 360;
		Vector3 headingCorrection = headingController.Update(Vector3.Cross(transform.right, BattleContext.PlayerShip.Position - Position), Time.fixedDeltaTime);
		m_rigidbody.AddTorque(headingCorrection * 0.2f);

		if (m_rigidbody.velocity.magnitude < 12) {
			m_rigidbody.AddRelativeForce(Vector3.right * 10, ForceMode.Force);
		} else {
			m_rigidbody.AddRelativeForce(Vector3.right * 80, ForceMode.Force);
		} 
		
		if (m_rigidbody.velocity.magnitude > 15) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 15;
		}
		if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.PlayerShip.Position - Position)) > 75) {
			m_state = RamShipState.Stopping;
		}
	}

	private void Stopping() {
		m_needRotationSpeed = 120;
		if (m_rigidbody.velocity.magnitude > 0.5f) {
			m_rigidbody.velocity *= 0.965f;
		} else {
			m_state = RamShipState.Aiming;
		}
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

	private enum RamShipState {
		Aiming,
		Stabilizing,
		Running,
		Stopping,
		Dead
	}

}
