using UnityEngine;

public class TimeBonus : IBonus {
	[SerializeField]
	private int m_giveSecondsValue, m_giveSecondsDispertion;

	private float m_giveSeconds;

	protected override void OnInitiate() {
		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		m_giveSeconds = MathHelper.Random.Next(m_giveSecondsDispertion * 2) - m_giveSecondsDispertion + m_giveSecondsValue;
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

	public float GiveSeconds {
		get {
			return m_giveSeconds;
		}
	}

}
