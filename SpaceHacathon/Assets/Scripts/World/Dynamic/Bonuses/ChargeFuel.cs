using UnityEngine;

public class ChargeFuel : IBonus {

	protected override void OnInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
	}

	protected override void OnDespawn(DespawnReason reason) {
	}

	protected override void OnFixedUpdateEntity() {
	}

	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override float DistanceToDespawn {
		get {
			return 50;
		}
	}

}
