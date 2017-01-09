using UnityEngine;

public class Explosion : MonoBehaviour {
	private ExplosionType m_explosionType;

	[SerializeField]
	private float m_lifeTime;
	[SerializeField]
	private ParticleSystem[] m_particles;

    private float m_timeToDie;

	public void Initiate(ExplosionType type) {
		gameObject.SetActive(false);
		m_explosionType = type;
	}

	public void Spawn(Vector3 position) {
		gameObject.SetActive(true);
		transform.position = position;
		foreach(ParticleSystem system in m_particles) {
            system.Play();
        }
        
		m_timeToDie = m_lifeTime;
	}

	private void Update() {
		m_timeToDie -= Time.deltaTime;
		if (m_timeToDie <= 0) {
			foreach(ParticleSystem system in m_particles) {
				system.Stop();
			}
			gameObject.SetActive(false);
		}
	}

	public ExplosionType ExplosionType {
		get {
			return m_explosionType;
		}
	}
}

public enum ExplosionType {
	PlayerShipExplosion,
	RocketExplosion,
	BlasterExplosion,
	MineExplosion
}
