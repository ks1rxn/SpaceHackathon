using UnityEngine;

public class HealthDrone : MonoBehaviour {
	[SerializeField]
	private GameObject m_beam;

	private HealthDroneState m_state;

	public void Initiate() {
		gameObject.SetActive(false);
	}

	public void Spawn(Vector3 position, float angle) {
		gameObject.SetActive(true);

		gameObject.transform.position = position;
		gameObject.transform.rotation = new Quaternion();
		gameObject.transform.Rotate(0, angle, 0);

		ToSleepState();
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void UpdateEntity() {
		switch (m_state) {
			case HealthDroneState.Sleep:
				break;
		}
	}

	private void ToSleepState() {
		m_state = HealthDroneState.Sleep;
		m_beam.SetActive(false);
	}

}

public enum HealthDroneState {
	Sleep
}