using UnityEngine;

public class TimeBonus : MonoBehaviour {
	[SerializeField]
	private int m_giveSecondsValue, m_giveSecondsDispertion;
	[SerializeField]
	private CollisionDetector m_collisionDetector;

	private float m_giveSeconds;

	public void Initiate() {
		m_collisionDetector.Initiate();
		m_collisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);

		IsAlive = false;
	}

	public void Spawn(Vector3 position) {
		IsAlive = true;
		transform.position = position;
		m_giveSeconds = MathHelper.Random.Next(m_giveSecondsDispertion * 2) - m_giveSecondsDispertion + m_giveSecondsValue;
	}

	public void UpdateState() {
	}

	private void OnTargetHit(GameObject other) {
		IsAlive = false;
	}

	public bool IsAlive {
		get {
			return gameObject.activeInHierarchy;
		}
		set {
			gameObject.SetActive(value);
		}
	}

	public float GiveSeconds {
		get {
			return m_giveSeconds;
		}
	}

}
