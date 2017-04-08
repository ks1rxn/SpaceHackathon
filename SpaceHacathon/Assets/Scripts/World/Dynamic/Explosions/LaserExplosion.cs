using UnityEngine;

public class LaserExplosion : IExplosion {
	[SerializeField]
	private float m_lifeTime;
	[SerializeField]
	private ParticleSystem[] m_particles;

    private float m_timeToDie;

	protected override void OnInitiate() {
	}

	protected override void OnSpawn(Vector3 position, Vector3 angle) {
		foreach(ParticleSystem system in m_particles) {
            system.Play();
        }
        
		m_timeToDie = m_lifeTime;
	}

	protected override void OnDespawn(DespawnReason reason) {

	}

	protected override void OnFixedUpdateEntity() {
		m_timeToDie -= Time.deltaTime;
		if (m_timeToDie <= 0) {
			foreach(ParticleSystem system in m_particles) {
				system.Stop();
			}
			Despawn(DespawnReason.TimeOff);
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 30;
		}
	}

}
