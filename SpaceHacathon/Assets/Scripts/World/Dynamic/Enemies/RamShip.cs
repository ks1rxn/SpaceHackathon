using UnityEngine;

public class RamShip : IEnemyShip {
	[SerializeField]
	private Transform m_hull;

	private RamShipState m_state;
	private VectorPid m_headingController;

	private float m_rotationSpeed;
	private float m_needRotationSpeed;

	[SerializeField, Range(0f, 1.0f)]
	private float m_rotationSensitivityRun;
	[SerializeField, Range(0f, 1.0f)]
	private float m_rotationSensitivityAim;
	[SerializeField]
	private Vector3 m_headingParams;
	[SerializeField, Range(0f, 30.0f)]
	private float m_finishAimAngle;
	[SerializeField, Range(0f, 1.0f)]
	private float m_finishAimAngVelocity;
	[SerializeField]
	private float m_headingMaxOnRun;
	[SerializeField, Range(0f, 50.0f)]
	private float m_maxRunSpeed;
	[SerializeField]
	private Vector2 m_acceleration;
	[SerializeField, Range(0f, 180.0f)]
	private float m_angleToStartStop;
	[SerializeField, Range(0f, 1.0f)]
	private float m_stopAcceleration;
	[SerializeField]
	private float m_distanceToDie;

	protected override void OnPhysicBodyInitiate() {
		m_headingController = new VectorPid(m_headingParams);

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.SpaceMine, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.Missile, OnOtherShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_rotationSpeed = 0;
		m_needRotationSpeed = 0;

		m_state = RamShipState.Aiming;
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.PlayerShipExplosion(Position);
		BattleContext.EnemiesController.OnRamShipDie();
	}

	private void OnOtherShipHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
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
		if (Rigidbody.velocity.magnitude > 0.1f) {
			Rigidbody.velocity *= 0.5f;
		}
		Vector3 headingCorrection = m_headingController.Update(Vector3.Cross(transform.right, BattleContext.PlayerShip.Position - Position), Time.fixedDeltaTime);
		Rigidbody.AddTorque(headingCorrection * m_rotationSensitivityAim);
		if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.PlayerShip.Position - Position)) < m_finishAimAngle &&
			Rigidbody.angularVelocity.magnitude < m_finishAimAngVelocity) {

			if (!IsLauncherOnMyWay()) {
				m_state = RamShipState.Running;
			}
		}
	}

	private void Stabilizing() {
		m_needRotationSpeed = 50;
		if (Rigidbody.angularVelocity.magnitude > 0.1f) {
			Rigidbody.angularVelocity *= 0.5f;
		} else {
			m_state = RamShipState.Running;
		}
	}

	private void Running() {
		if (IsLauncherOnMyWay()) {
			m_state = RamShipState.Stopping;
			return;
		}
		m_needRotationSpeed = 360;
		Vector3 headingCorrection = m_headingController.Update(Vector3.Cross(transform.right, BattleContext.PlayerShip.Position - Position), Time.fixedDeltaTime);
		headingCorrection.y = Mathf.Clamp(headingCorrection.y, -m_headingMaxOnRun, m_headingMaxOnRun);
		Rigidbody.AddTorque(headingCorrection * m_rotationSensitivityRun);

		if (Rigidbody.velocity.magnitude < 0.75f * m_maxRunSpeed) {
			Rigidbody.AddRelativeForce(Vector3.right * m_acceleration[0], ForceMode.Force);
		} else {
			Rigidbody.AddRelativeForce(Vector3.right * m_acceleration[1], ForceMode.Force);
		} 
		
		if (Rigidbody.velocity.magnitude > m_maxRunSpeed) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * m_maxRunSpeed;
		}
		if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.PlayerShip.Position - Position)) > m_angleToStartStop) {
			m_state = RamShipState.Stopping;
		}
	}

	private void Stopping() {
		m_needRotationSpeed = 120;
		if (Rigidbody.velocity.magnitude > 0.5f) {
			Rigidbody.velocity *= m_stopAcceleration;
		} else {
			m_state = RamShipState.Aiming;
		}
	}

	private bool IsLauncherOnMyWay() {
		foreach (DroneCarrier ship in BattleContext.EnemiesController.DroneCarriers) {
			if (Vector3.Distance(ship.Position, Position) < Vector3.Distance(Position, BattleContext.PlayerShip.Position)) { 
				RaycastHit hit;
				Ray ray = new Ray(Position, transform.right);
				if (Physics.Raycast(ray, out hit)) {
					Transform objectHit = hit.transform;
					if (objectHit.GetComponent<DroneCarrier>() != null) {
						m_state = RamShipState.Stopping;
						return true;
					}
				}
			}
		}
		return false;
	}

	public RamShipState State {
		get {
			return m_state;
		}
	}

	protected override float DistanceToDespawn {
		get {
			return m_distanceToDie;
		}
	}

}

public enum RamShipState {
	Aiming,
	Stabilizing,
	Running,
	Stopping
}
