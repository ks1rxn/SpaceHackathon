using UnityEngine;

public class ChargeFuel : IBonus {

	protected override void OnInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
	}

	protected override void OnDespawn() {
	}

	protected override void OnFixedUpdateEntity() {
	}

	private void OnTargetHit(GameObject other) {
		Despawn();
	}

	protected override float DistanceToDespawn {
		get {
			return 50;
		}
	}

}
