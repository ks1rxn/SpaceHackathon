using UnityEngine;

public class Explosion : MonoBehaviour {
	[SerializeField]
	private float m_lifeTime;
	[SerializeField]
	private ParticleSystem[] m_particles;

    private float m_timeToDie;

	public void Spawn(Vector3 position) {
		transform.position = position;
		foreach(ParticleSystem system in m_particles) {
            system.Play();
        }
        
		m_timeToDie = m_lifeTime;
	}

	protected void Update() {
		m_timeToDie -= Time.deltaTime;
		if (m_timeToDie <= 0) {
			Destroy(gameObject);
		}
	}

}
