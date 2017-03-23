using UnityEngine;

public class BattleCamera : MonoBehaviour {
	private Vector3 m_speed;
	private readonly VectorPid m_speedController = new VectorPid(50000f, 20000, 45000);

	public void UpdateEntity() {
		Vector3 neededPosition = BattleContext.PlayerShip.Position + BattleContext.PlayerShip.SpeedVector * 0.2f;
		neededPosition.y = 7.5f;
		neededPosition.z -= 9;

		Vector3 speedCorrection = m_speedController.Update(neededPosition - transform.position, Time.fixedDeltaTime);
		m_speed += 0.0003f * speedCorrection * Time.fixedDeltaTime;
		transform.Translate(0.4f * m_speed * Time.fixedDeltaTime);
		Vector3 pos = transform.position;
		pos.y = 7.5f;
		transform.position = pos;

		transform.eulerAngles = new Vector3(50, 0, 0);
	}

}
