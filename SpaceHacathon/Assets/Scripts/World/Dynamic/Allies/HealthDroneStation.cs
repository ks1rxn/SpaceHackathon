using UnityEngine;

public class HealthDroneStation : IAlly {

	public override void Initiate() {
		base.Initiate();
	}

	public override void Spawn(Vector3 position, float rotation) {
		base.Spawn(position, rotation);
	}

	public override void UpdateEntity() {
		base.UpdateEntity();
	}

	protected override float DistanceFromPlayerToDie {
		get {
			return 60;
		}
	}

}
