using System.Collections;
using UnityEngine;

public class CarrierRocket : IBullet {
	private SettingsCarrierRocket m_settings;

	private float m_detonatorActivateTime;

	[SerializeField]
	private ParticleSystem m_trail;

	private float m_power;
	private bool m_inactive;

	private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

	private float m_maxSpeed;
	private float m_maxRotationSpeed;

	private Vector3 m_target;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.CarrierRocket;

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_trail.Stop();

		m_maxSpeed = m_settings.MaxSpeed;
		m_maxRotationSpeed = m_settings.MaxRotationSpeed;

		m_target = BattleContext.BattleManager.Director.PlayerShip.Position;

		StartCoroutine(LaunchProcess());
	}

	protected override void OnDespawn(DespawnReason reason) {
		Vector3 cloudPosition = Position;
		cloudPosition.y = 0;
		BattleContext.BattleManager.ExplosionsController.RocketExplosion(Position);
		BattleContext.BattleManager.EffectsController.SpawnSlowingCloud(cloudPosition);
	}

	private IEnumerator LaunchProcess() {
		m_power = 1.1f;
		m_inactive = true;
		Rigidbody.drag = 2.0f;
		GetComponent<Collider>().enabled = false;

		Rigidbody.AddExplosionForce(340 * Rigidbody.mass, transform.position + new Vector3(0, -0.04f, 0), 1);
		yield return new WaitForSeconds(0.75f);

		m_trail.Play();

		GetComponent<Collider>().enabled = true;
		Rigidbody.drag = 0.0f;
		m_inactive = false;

		m_target = BattleContext.BattleManager.Director.PlayerShip.Position;
	}

	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override void OnFixedUpdateEntity() {
		if (m_inactive) {
			return;
		}

		Vector3 angularVelocityError = Rigidbody.angularVelocity * -1;         
        Vector3 angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.fixedDeltaTime);
 
        Rigidbody.AddTorque(angularVelocityCorrection * 1.0f);
 
        Vector3 desiredHeading = m_target - transform.position; 
        Vector3 currentHeading = transform.up;
 
        Vector3 headingError = Vector3.Cross(currentHeading, desiredHeading);
        Vector3 headingCorrection = headingController.Update(headingError, Time.fixedDeltaTime);
 
        Rigidbody.AddTorque(headingCorrection * m_settings.RotationCoefficient);
		if (Rigidbody.angularVelocity.magnitude > m_maxRotationSpeed) {
			Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * m_maxRotationSpeed;
		}

		if (m_power < 120) {
			m_power += Time.fixedDeltaTime * Mathf.Min(m_power * m_power * m_power, 50);
		}
		Rigidbody.AddRelativeForce(0, m_power, 0);
		if (Rigidbody.velocity.magnitude > m_maxSpeed) {
			Rigidbody.velocity = Rigidbody.velocity.normalized * m_maxSpeed;
		}

		if (transform.position.y <= 0) {
			Despawn(DespawnReason.TargetReached);
		}
	}

	protected override float DistanceToDespawn {
		get {
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}

}
