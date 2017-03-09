using UnityEngine;

public class Explosion : IExplosion {
	[SerializeField]
	private ExplosionType m_explosionType;

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

	protected override void OnDespawn() {

	}

	protected override void OnFixedUpdateEntity() {
		m_timeToDie -= Time.deltaTime;
		if (m_timeToDie <= 0) {
			foreach(ParticleSystem system in m_particles) {
				system.Stop();
			}
			Despawn();
		}
	}

	public ExplosionType ExplosionType {
		get {
			return m_explosionType;
		}
	}

	protected override float DistanceToDespawn {
		get {
			return 30;
		}
	}

}

public enum ExplosionType {
	PlayerShipExplosion,
	RocketExplosion,
	BlasterExplosion,
	MineExplosion
}
