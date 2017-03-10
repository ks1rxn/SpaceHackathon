using UnityEngine;

public class HealthDrone : ISpawnable {
	[SerializeField]
	private GameObject m_beam;

	private HealthDroneState m_state;

	protected override void OnInitiate() {
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		ToSleepState();
	}

	protected override void OnDespawn(DespawnReason reason) {
	}

	protected override void OnFixedUpdateEntity() {
		switch (m_state) {
			case HealthDroneState.Sleep:
				break;
		}
	}

	private void ToSleepState() {
		m_state = HealthDroneState.Sleep;
		m_beam.SetActive(false);
	}

	protected override float DistanceToDespawn {
		get {
			return 100;
		}
	}

}

public enum HealthDroneState {
	Sleep
}