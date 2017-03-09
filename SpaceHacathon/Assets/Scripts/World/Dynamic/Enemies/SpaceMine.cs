using UnityEngine;

public class SpaceMine : IEnemyShip {
	[SerializeField]
	private GameObject m_waitingIndicator;
	[SerializeField]
	private GameObject m_armedIndicator;

	private SpaceMineState m_state;

	private readonly VectorPid m_speedController = new VectorPid(1.244681f, 0.1f, 1.1f);
	private readonly FloatPid m_yStabilizer = new FloatPid(1.244681f, 0.1f, 1.1f);

	private Vector3 m_waitingPosition;
	private float m_speedValue;

	[SerializeField]
	private MeshRenderer m_hullRenderer;
	[SerializeField]
	private Material[] m_materials;
	[SerializeField]
	private LineRenderer m_lineRenderer;

	protected override void OnPhysicBodyInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.SpaceMine, OnOtherShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_waitingIndicator.SetActive(true);
		m_armedIndicator.SetActive(false);

		m_state = SpaceMineState.Waiting;
		m_hullRenderer.material = m_materials[0];

		position.y = -2.5f + (float) MathHelper.Random.NextDouble() - 0.5f;
		transform.position = position;

		m_waitingPosition = position;
		m_waitingPosition.y = -3 + MathHelper.Random.Next(1);
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.ExplosionsController.MineExplosion(Position);
	}

	private void OnOtherShipHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case SpaceMineState.Waiting:
				Waiting();
				break;
			case SpaceMineState.Chasing:
				Chasing();
				break;
		}
	}

	private void LateUpdate() {
		switch (m_state) {
			case SpaceMineState.Waiting:
				m_lineRenderer.SetPosition(0, Position);
				m_lineRenderer.SetPosition(1, Position);
				break;
			case SpaceMineState.Chasing:
				Vector3 pos = Position;
				pos.y = 0;
				m_lineRenderer.SetPosition(0, Position);
				Vector3 toTarget = (BattleContext.PlayerShip.Position - pos).normalized * 1.31f;
				m_lineRenderer.SetPosition(1, BattleContext.PlayerShip.Position - toTarget);
				break;
		}
	}

	private void Waiting() {
		Vector3 projectionToPlane = Position;
		projectionToPlane.y = 0;
		if (Vector3.Distance(BattleContext.PlayerShip.Position, projectionToPlane) < 7) {
			m_waitingIndicator.SetActive(false);
			m_armedIndicator.SetActive(true);

			m_hullRenderer.material = m_materials[1];
			m_state = SpaceMineState.Chasing;
			m_speedValue = 0.0f;
		}
		transform.Rotate(0, Time.fixedDeltaTime * 35, 0);
		Vector3 speedCorrection = m_speedController.Update(m_waitingPosition - Position, Time.fixedDeltaTime);
		Rigidbody.AddForce(speedCorrection * 0.5f);
		if (Mathf.Abs(m_waitingPosition.y - Position.y) < 0.1f) {
			m_waitingPosition.y = -5 - m_waitingPosition.y;
		}
	}

	private void Chasing() {
		Vector3 projectionToPlane = Position;
		projectionToPlane.y = 0;

		Vector3 speedCorrection = m_speedController.Update(BattleContext.PlayerShip.Position - Position, Time.fixedDeltaTime);
		Rigidbody.AddForce(speedCorrection * m_speedValue * 0.9f);

		float yCorrection = m_yStabilizer.Update(-Position.y, Time.fixedDeltaTime);
		Rigidbody.AddForce(0, yCorrection * 0.5f, 0);

		float distCoef = Mathf.Clamp((BattleContext.PlayerShip.Position - Position).magnitude, 3, 8);
		if (Rigidbody.velocity.magnitude > distCoef) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * distCoef;
		}
		if (m_speedValue < 0.5f) {
			m_speedValue += Time.fixedDeltaTime;
		}
		if (Vector3.Distance(BattleContext.PlayerShip.Position, projectionToPlane) > 9) {
			m_waitingIndicator.SetActive(true);
			m_armedIndicator.SetActive(false);

			m_hullRenderer.material = m_materials[0];
			m_state = SpaceMineState.Waiting;

			m_waitingPosition = Position;
			m_waitingPosition.y = -3 + MathHelper.Random.Next(1);
		}
	}

	public SpaceMineState State {
		get {
			return m_state;
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 40;
		}
	}

}

public enum SpaceMineState {
	Waiting,
	Chasing
}