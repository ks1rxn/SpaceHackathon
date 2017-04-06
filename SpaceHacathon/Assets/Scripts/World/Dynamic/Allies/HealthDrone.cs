using UnityEngine;

public class HealthDrone : IPhysicBody {
	[SerializeField]
	private GameObject m_beam;

	private Transform m_base;

	private HealthDroneState m_state;
	private readonly VectorPid positionController = new VectorPid(8.244681f, 0.1f, 3.5f);
	private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(18.244681f, 0, 0.06382979f);
	private Vector3 m_speed;
	private Vector3 m_rotationSpeed;
	private Vector3 m_positionOnPlayer;
	private float m_timeToSwitch;

	protected override void OnPhysicBodyInitiate() {

	}

	public void SetBase(Transform station) {
		m_base = station;
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		ToSleepState();
	}

	protected override void OnDespawn(DespawnReason reason) {
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case HealthDroneState.Sleep:
				break;
			case HealthDroneState.MoveToPlayer:
				PerformMoveToPlayerState();
				break;
			case HealthDroneState.MoveToBase:
				PerformMoveToBaseState();
				break;
		}
	}

	private void ToSleepState() {
		m_state = HealthDroneState.Sleep;
		m_beam.SetActive(false);
	}

	public void ToMoveToPlayerState() {
		m_state = HealthDroneState.MoveToPlayer;
		m_positionOnPlayer = MathHelper.GetPointOnSphere(Vector3.zero, 1.5f, 2.0f);
		m_timeToSwitch = 2.5f;
	}

	public void ToMoveToBaseState() {
		m_state = HealthDroneState.MoveToBase;
		m_beam.SetActive(false);
	}

	private void PerformMoveToPlayerState() {
		Vector3 playerPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
		Vector3 positionCorrection = positionController.Update(playerPosition + m_positionOnPlayer - Position, Time.fixedDeltaTime);
	    m_speed += positionCorrection * Time.fixedDeltaTime;
		if (m_speed.magnitude > 8) {
			m_speed = m_speed.normalized * 8;
		}
		transform.position += m_speed * Time.fixedDeltaTime;
       
        Vector3 desiredHeading = playerPosition - Position; 
        Vector3 currentHeading = transform.forward;
    
        Vector3 angularVelocityCorrection = angularVelocityController.Update(-Rigidbody.angularVelocity, Time.fixedDeltaTime);
		Rigidbody.AddTorque(angularVelocityCorrection);

        Vector3 headingError = Vector3.Cross(currentHeading, desiredHeading);
        Vector3 headingCorrection = headingController.Update(headingError, Time.fixedDeltaTime);
		Rigidbody.AddTorque(headingCorrection * 12.0f);

		m_beam.SetActive((playerPosition - Position).magnitude < 3 && Rigidbody.angularVelocity.magnitude < 2f);

		m_timeToSwitch -= Time.fixedDeltaTime;
		if (m_timeToSwitch < 0) {
			m_timeToSwitch = 2.5f;
			m_positionOnPlayer = MathHelper.GetPointOnSphere(Vector3.zero, 1.5f, 2.0f);
		}
	}

	private void PerformMoveToBaseState() {
		Rigidbody.angularVelocity = Vector3.zero;
		Vector3 positionCorrection = positionController.Update(m_base.position - Position, Time.fixedDeltaTime);
	    m_speed += positionCorrection * Time.fixedDeltaTime;
		if (m_speed.magnitude > 8) {
			m_speed = m_speed.normalized * 8;
		}
		transform.position += m_speed * Time.fixedDeltaTime;
	}

	protected override float DistanceToDespawn {
		get {
			return 100;
		}
	}

}

public enum HealthDroneState {
	Sleep,
	MoveToPlayer,
	MoveToBase
}