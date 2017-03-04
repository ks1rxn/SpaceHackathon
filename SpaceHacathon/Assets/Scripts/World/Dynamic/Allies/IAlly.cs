using UnityEngine;

public abstract class IAlly : MonoBehaviour {

	public virtual void Initiate() {
		IsAlive = false;
	}

	public virtual void Spawn(Vector3 position, float rotation) {
		IsAlive = true;

		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(0, rotation, 0);
	}

	public virtual void UpdateEntity() {
		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) > DistanceFromPlayerToDie) {
			IsAlive = false;
		}
	}

	public virtual void Kill() {
		IsAlive = false;
	}

	public virtual bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
			OnDie();
		}
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

	protected abstract float DistanceFromPlayerToDie { get; }

	protected abstract void OnDie();
}
