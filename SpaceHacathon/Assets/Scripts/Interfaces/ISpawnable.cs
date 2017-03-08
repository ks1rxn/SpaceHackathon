using UnityEngine;

public abstract class ISpawnable : IEntity {
	public CollisionDetector CollisionDetector;

	public override void Initiate() {
		gameObject.SetActive(false);

		OnInitiate();
	}

	public void Spawn(Vector3 position, float angle) {
		gameObject.SetActive(true);

		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(0, angle, 0);

		OnSpawn(position, angle);
	}

	public void Despawn() {
		gameObject.SetActive(false);

		OnDespawn();
	}

	public bool IsSpawned() {
		return gameObject.activeInHierarchy;
	}

	public override void FixedUpdateEntity() {
		if (Vector3.Distance(BattleContext.Director.PlayerPosition, Position) > DistanceToDespawn) {
			Despawn();
		}
		OnFixedUpdateEntity();
	}

	protected abstract void OnInitiate();

	protected abstract void OnSpawn(Vector3 position, float angle);

	protected abstract void OnDespawn();

	protected abstract void OnFixedUpdateEntity();

	protected abstract float DistanceToDespawn { get; }

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}