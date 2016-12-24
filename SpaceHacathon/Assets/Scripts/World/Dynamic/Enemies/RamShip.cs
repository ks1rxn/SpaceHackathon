using UnityEngine;

public class RamShip : IEnemyShip {
	[SerializeField]
	private Transform m_hull;
	private RamShipState m_state;
	private readonly VectorPid headingController = new VectorPid(1.244681f, 0.1f, 1.1f);
	private float m_rotationSpeed;
	private float m_needRotationSpeed;

	public override void Initiate() {
		base.Initiate();
	}

	public override void Spawn(Vector3 position, float angle) {
		base.Spawn(position, angle);

		m_rotationSpeed = 0;
		m_needRotationSpeed = 0;

		m_state = RamShipState.Aiming;
	}

	public override void Kill() {
		base.Kill();
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
	}

	private void OnTriggerEnter(Collider other) { 
		if (other.GetComponent<PlayerShip>() != null) {
			Kill();
		}
    }

	public override void UpdateShip() {
		base.UpdateShip();

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

			if (!HittingLauncher()) {
				m_state = RamShipState.Running;
			}
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
		if (HittingLauncher()) {
			m_state = RamShipState.Stopping;
			return;
		}
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

	private bool HittingLauncher() {
		foreach (IEnemyShip ship in BattleContext.EnemiesController.Ships) {
			if (ship is RocketLauncher) {
				if (Vector3.Distance(ship.Position, Position) < Vector3.Distance(Position, BattleContext.PlayerShip.Position)) { 
					RaycastHit hit;
					Ray ray = new Ray(Position, transform.right);
					if (Physics.Raycast(ray, out hit)) {
						Transform objectHit = hit.transform;
						if (objectHit.GetComponent<RocketLauncher>() != null) {
							m_state = RamShipState.Stopping;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public override bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
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
