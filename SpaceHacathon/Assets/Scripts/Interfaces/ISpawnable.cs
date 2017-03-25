using UnityEngine;

public abstract class ISpawnable : IEntity {
	public CollisionDetector CollisionDetector;

	public override void Initiate() {
		gameObject.SetActive(false);

		OnInitiate();
	}

	public void Spawn(Vector3 position, float angle) {
		Vector3 angleVec = new Vector3(0, angle, 0);
		Spawn(position, angleVec);
	}

	public void Spawn(Vector3 position, Vector3 angle) {
		gameObject.SetActive(true);

		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(angle.x, angle.y, angle.z);

		OnSpawn(position, angle);
	}

	public void Despawn(DespawnReason reason) {
		gameObject.SetActive(false);

		OnDespawn(reason);
	}

	public bool IsSpawned() {
		return gameObject.activeInHierarchy;
	}

	public override void FixedUpdateEntity() {
		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) > DistanceToDespawn) {
			Despawn(DespawnReason.OutOfRange);
		}
		OnFixedUpdateEntity();
	}

	protected abstract void OnInitiate();

	protected abstract void OnSpawn(Vector3 position, Vector3 angle);

	protected abstract void OnDespawn(DespawnReason reason);

	protected abstract void OnFixedUpdateEntity();

	protected abstract float DistanceToDespawn { get; }

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}

public enum DespawnReason {
	OutOfRange,
	TimeOff,
	ParentDespawn,
	TargetReached,
	Kill
}