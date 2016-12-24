
using UnityEngine;

public abstract class IEnemyShip : MonoBehaviour {
	protected Rigidbody m_rigidbody;

	public virtual void Initiate() {
		m_rigidbody = GetComponent<Rigidbody>();
		IsAlive = false;
	}

	public virtual void Spawn(Vector3 position, float rotation) {
		IsAlive = true;

		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(0, rotation, 0);

		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
	}

	public virtual void UpdateShip() {
		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) > 80) {
			IsAlive = false;
		}
	}

	public virtual void Kill() {
		IsAlive = false;
	}

	public abstract bool IsAlive { get; set; }

	public Vector3 Position {
		get {
			return transform.position;
		}
	}
}
