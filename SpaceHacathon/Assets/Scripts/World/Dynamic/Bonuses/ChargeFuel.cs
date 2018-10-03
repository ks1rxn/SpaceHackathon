using SpaceHacathon.Helpers;
using UnityEngine;

public class ChargeFuel : IBonus {
	private Vector3 m_rotationVector;
	private float m_rotationSpeed;

	protected override void OnInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		Vector3 intialRotation = new Vector3((float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f, (float)MathHelper.Random.NextDouble() - 0.5f).normalized;
		transform.Rotate(intialRotation, MathHelper.Random.Next(360));

		m_rotationVector = new Vector3(0, 1, 0);
		m_rotationSpeed = ((float) MathHelper.Random.NextDouble() - 0.5f) * 2f;
	}

	protected override void OnDespawn(DespawnReason reason) {
	}

	protected override void OnFixedUpdateEntity() {
		transform.Rotate(m_rotationVector, m_rotationSpeed);
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
