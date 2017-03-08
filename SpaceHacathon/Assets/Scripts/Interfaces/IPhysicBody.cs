using UnityEngine;

public abstract class IPhysicBody : ISpawnable {
	protected Rigidbody Rigidbody;

	protected override void OnInitiate() {
		Rigidbody = GetComponent<Rigidbody>();

		OnPhysicBodyInitiate();
	}

	protected override void OnSpawn(Vector3 position, float angle) {
		Rigidbody.velocity = Vector3.zero;
		Rigidbody.angularVelocity = Vector3.zero;

		OnPhysicBodySpawn(position, angle);
	}

	protected abstract void OnPhysicBodyInitiate();

	protected abstract void OnPhysicBodySpawn(Vector3 position, float angle);

}