using UnityEngine;

public class SpaceMine : IEnemyShip {
	private SettingsSpaceMine m_settings;

	[SerializeField]
	private GameObject m_waitingIndicator;
	[SerializeField]
	private GameObject m_waitingCircle;
	[SerializeField]
	private GameObject m_armedIndicator;

	private SpaceMineState m_state;

	private readonly VectorPid m_speedController = new VectorPid(1.244681f, 0.1f, 1.1f);
	private readonly FloatPid m_yStabilizer = new FloatPid(1.244681f, 0.1f, 1.1f);

	private Vector3 m_waitingPosition;
	private float m_speedValue;

	[SerializeField]
	private GameObject m_hull;
	private MeshRenderer m_hullRenderer;
	[SerializeField]
	private Material[] m_materials;
	[SerializeField]
	private LineRenderer m_lineRenderer;

	private float m_param;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.SpaceMine;

		m_hullRenderer = m_hull.GetComponent<MeshRenderer>();

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnOtherShipHit);
		CollisionDetector.RegisterListener(CollisionTags.SpaceMine, OnOtherShipHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		position.y = -0.6f;
		transform.position = position;

		ToWaitingState();
	}

	protected override void OnDespawn(DespawnReason reason) {
		BattleContext.BattleManager.ExplosionsController.MineExplosion(Position);
	}

	private void OnOtherShipHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case SpaceMineState.Waiting:
				PerformWaitingState();
				break;
			case SpaceMineState.Chasing:
				PerformChasingState();
				break;
		}
	}

	private void ToWaitingState() {
		m_state = SpaceMineState.Waiting;

		m_waitingIndicator.SetActive(true);
		m_armedIndicator.SetActive(false);

		m_hullRenderer.material = m_materials[0];

		m_waitingPosition = Position;
		m_waitingPosition.y = -0.6f;
	}

	private void PerformWaitingState() {
		Vector3 projectionToPlane = Position;
		projectionToPlane.y = 0;
		if (Vector3.Distance(BattleContext.BattleManager.Director.PlayerShip.Position, projectionToPlane) < 7) {
			ToChasingState();
		}
		transform.Rotate(0, Time.fixedDeltaTime * 35, 0);
		Vector3 speedCorrection = m_speedController.Update(m_waitingPosition - Position, Time.fixedDeltaTime);
		Rigidbody.AddForce(speedCorrection * 0.5f);
//		if (Mathf.Abs(m_waitingPosition.y - Position.y) < 0.1f) {
//			m_waitingPosition.y = -5 - m_waitingPosition.y;
//		}
		
		float diff = -0.6f - Position.y;
		Vector3 circlePos = m_waitingCircle.transform.localPosition;
		circlePos.y = 0.6f + diff;
		m_waitingCircle.transform.localPosition = circlePos;
	}

	private void ToChasingState() {
		m_state = SpaceMineState.Chasing;
	
		m_waitingIndicator.SetActive(false);
		m_armedIndicator.SetActive(true);

		m_hullRenderer.material = m_materials[1];
		m_speedValue = 0.0f;
		
		m_param = 0;	
	}

	private void PerformChasingState() {
		Vector3 projectionToPlane = Position;
		projectionToPlane.y = 0;

		Vector3 speedCorrection = m_speedController.Update(BattleContext.BattleManager.Director.PlayerShip.Position - Position, Time.fixedDeltaTime);
		Rigidbody.AddForce(speedCorrection * m_speedValue * 0.9f);

		float yCorrection = m_yStabilizer.Update(-Position.y, Time.fixedDeltaTime);
		Rigidbody.AddForce(0, yCorrection * 0.5f, 0);

		float distCoef = Mathf.Clamp((BattleContext.BattleManager.Director.PlayerShip.Position - Position).magnitude, 3, 8);
		if (Rigidbody.velocity.magnitude > distCoef) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * distCoef;
		}
		if (m_speedValue < 0.5f) {
			m_speedValue += Time.fixedDeltaTime;
		}
		if (Vector3.Distance(BattleContext.BattleManager.Director.PlayerShip.Position, projectionToPlane) > 9) {
			ToWaitingState();
		}
		float dist = Vector3.Distance(BattleContext.BattleManager.Director.PlayerShip.Position, projectionToPlane);
		float coef = Mathf.Clamp((7 - dist) * (7 - dist), 0, 49) / 49f * 1.5f;
		float speed = Mathf.Clamp(8 - dist, 1, 6) / 1f;
		m_param += Time.fixedDeltaTime * speed;
		float size = Mathf.Sin(m_param) * Mathf.Sin(m_param) * coef;
		Vector3 scale = new Vector3(size + 1, size + 1, size + 1);
		m_hull.transform.localScale = scale;
		m_armedIndicator.transform.localScale = scale;
	}

	///<summary>
	/// Draw line from mine to PlayerShip.
	/// LateUpdate is used because in FixedUpdate line end point (on PlayerShip) was not correct.
	///</summary>
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
				Vector3 toTarget = (BattleContext.BattleManager.Director.PlayerShip.Position - pos).normalized * 1.31f;
				m_lineRenderer.SetPosition(1, BattleContext.BattleManager.Director.PlayerShip.Position - toTarget);
				break;
		}
	}

	public SpaceMineState State {
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

public enum SpaceMineState {
	Waiting,
	Chasing
}