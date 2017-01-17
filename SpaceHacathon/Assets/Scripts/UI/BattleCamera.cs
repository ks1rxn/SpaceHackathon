using UnityEngine;

public class BattleCamera : MonoBehaviour {
	[SerializeField]
	private Transform m_camera;

	private Vector3 m_speed;
	private Vector3 m_actualSpeed;

	private readonly VectorPid m_speedController = new VectorPid(50000f, 20000, 45000);
//	private readonly VectorPid m_speedController = new VectorPid(50000f, 20000, 65000);

	private float m_ySpeed;
	private readonly FloatPid m_yController = new FloatPid(1.5f, 0, 9f);

	protected void Awake() {
		BattleContext.BattleCamera = this;
	}

	protected void FixedUpdate() {
		Vector3 neededPosition = BattleContext.PlayerShip.Position + BattleContext.PlayerShip.SpeedValue * 0.2f;
		neededPosition.y = 7.5f;
		neededPosition.z -= 9;

		Vector3 speedCorrection = m_speedController.Update(neededPosition - transform.position, Time.fixedDeltaTime);
		m_speed += 0.0003f * speedCorrection * Time.fixedDeltaTime;
//		m_speed += 0.0001f * speedCorrection * Time.fixedDeltaTime;
		transform.Translate(0.4f * m_speed * Time.fixedDeltaTime);
		Vector3 pos = transform.position;
		pos.y = 7.5f;
		transform.position = pos;

		transform.eulerAngles = new Vector3(50, 0, 0);

//		float yCorrection = m_yController.Update(BattleContext.PlayerShip.SpeedValue + m_camera.transform.localPosition.z, Time.fixedDeltaTime);
//		m_ySpeed += yCorrection * Time.fixedDeltaTime;
//		Vector3 pos = m_camera.localPosition;
//		pos.z -= 0.1f * m_ySpeed * Time.fixedDeltaTime;
//		m_camera.localPosition = pos;

//		Vector3 position = BattleContext.PlayerShip.transform.position;
//		position.y = 3.5f;
//		position.z -= 4;
//		transform.position = position;
//		transform.eulerAngles = new Vector3(50, 0, 0);
	}

}
