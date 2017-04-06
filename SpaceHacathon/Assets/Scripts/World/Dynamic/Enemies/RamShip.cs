using UnityEngine;

public class RamShip : IEnemyShip {
	private SettingsRamShip m_settings;

	[SerializeField]
	private Transform m_hull;
	[SerializeField]
	private GameObject m_shield;

	private RamShipState m_state;
	private VectorPid m_headingController;

	private float m_rotationSpeed;
	private float m_needRotationSpeed;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.RamShip;

		m_headingController = new VectorPid(m_settings.HeadingParamsX, m_settings.HeadingParamsY, m_settings.HeadingParamsZ);

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnPlayerShipHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RocketShip, OnOtherShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_rotationSpeed = 0;
		m_needRotationSpeed = 0;

		ToAimingState();
	}

	protected override void OnDespawn(DespawnReason reason) {
		if (reason == DespawnReason.Kill) {
			BattleContext.BattleManager.ExplosionsController.PlayerShipExplosion(Position);
		}
		BattleContext.BattleManager.EnemiesController.OnRamShipDie();
	}

	private void OnPlayerShipHit(GameObject other) {
		if (m_state == RamShipState.Running) {
			Despawn(DespawnReason.Kill);
		}
		PlayerShip player = other.GetComponent<PlayerShip>();
		if (player != null && player.State == ShipState.InCharge) {
			Despawn(DespawnReason.Kill);
		}
	}

	private void OnOtherShipHit(GameObject other) {
		if (m_state == RamShipState.Running) {
			Despawn(DespawnReason.Kill);
		}
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case RamShipState.Aiming:
				PerformAimingState();
				break;
			case RamShipState.Running:
				PerformRunningState();
				break;
			case RamShipState.Stopping:
				PerformStoppingState();
				break;
		}

		if (m_rotationSpeed < m_needRotationSpeed) {
			m_rotationSpeed += 180 * Time.fixedDeltaTime;
		} else if (m_rotationSpeed > m_needRotationSpeed) {
			m_rotationSpeed -= 180 * Time.fixedDeltaTime;
		}
		m_hull.Rotate(m_rotationSpeed * Time.fixedDeltaTime, 0, 0);
	}

	private void ToAimingState() {
		m_state = RamShipState.Aiming;

		m_shield.SetActive(false);
	}

	private void PerformAimingState() {
		m_needRotationSpeed = 90;
		if (Rigidbody.velocity.magnitude > 0.1f) {
			Rigidbody.velocity *= 0.5f;
		}
		Vector3 headingCorrection = m_headingController.Update(Vector3.Cross(transform.right, BattleContext.BattleManager.Director.PlayerShip.Position - Position), Time.fixedDeltaTime);
		Rigidbody.AddTorque(headingCorrection * m_settings.RotationSensitivityAim * Rigidbody.mass);
		if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.BattleManager.Director.PlayerShip.Position - Position)) < m_settings.FinishAimAngle &&
			Rigidbody.angularVelocity.magnitude < m_settings.FinishAimAngleVelocity) {

			if (!IsLauncherOnMyWay()) {
				ToRunningState();
			}
		}
	}

	private void ToRunningState() {
		m_state = RamShipState.Running;

		m_shield.SetActive(true);
	}

	private void PerformRunningState() {
		if (IsLauncherOnMyWay()) {
			ToStoppingState();
			return;
		}
		m_needRotationSpeed = 360;
		Vector3 headingCorrection = m_headingController.Update(Vector3.Cross(transform.right, BattleContext.BattleManager.Director.PlayerShip.Position - Position), Time.fixedDeltaTime);
		headingCorrection.y = Mathf.Clamp(headingCorrection.y, -m_settings.HeadingMaxOnRun, m_settings.HeadingMaxOnRun);
		Rigidbody.AddTorque(headingCorrection * m_settings.RotationSensitivityRun * Rigidbody.mass);

		if (Rigidbody.velocity.magnitude < 0.75f * m_settings.MaxRunSpeed) {
			Rigidbody.AddRelativeForce(Vector3.right * m_settings.AccelerationStart * Rigidbody.mass, ForceMode.Force);
		} else {
			Rigidbody.AddRelativeForce(Vector3.right * m_settings.AccelerationEnd * Rigidbody.mass, ForceMode.Force);
		} 
		
		if (Rigidbody.velocity.magnitude > m_settings.MaxRunSpeed) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * m_settings.MaxRunSpeed;
		}
		if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.BattleManager.Director.PlayerShip.Position - Position)) > m_settings.AngleToStartStop) {
			ToStoppingState();
		}
	}

	private void ToStoppingState() {
		m_state = RamShipState.Stopping;

		Rigidbody.angularVelocity = Vector3.zero;
		m_shield.SetActive(false);
	}

	private void PerformStoppingState() {
		m_needRotationSpeed = 120;
		if (Rigidbody.velocity.magnitude > 0.5f) {
			Rigidbody.velocity *= m_settings.BrakingAcceleration;
		} else {
			ToAimingState();
		}
	}

	private bool IsLauncherOnMyWay() {
		foreach (DroneCarrier ship in BattleContext.BattleManager.EnemiesController.DroneCarriers) {
			if (Vector3.Distance(ship.Position, Position) < Vector3.Distance(Position, BattleContext.BattleManager.Director.PlayerShip.Position)) { 
				RaycastHit hit;
				Ray ray = new Ray(Position, transform.right);
				if (Physics.Raycast(ray, out hit)) {
					Transform objectHit = hit.transform;
					if (objectHit.GetComponent<DroneCarrier>() != null) {
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
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}

}

public enum RamShipState {
	Aiming,
	Running,
	Stopping
}
