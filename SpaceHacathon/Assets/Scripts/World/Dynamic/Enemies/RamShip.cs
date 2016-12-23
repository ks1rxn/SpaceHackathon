using UnityEngine;

public class RamShip : MonoBehaviour, IEnemyShip {
	private Rigidbody m_rigidbody;

	private bool m_inCharge;
	private readonly VectorPid headingController = new VectorPid(1.244681f, 0.1f, 1.1f);

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position, float angle) {
		transform.position = position;

		m_inCharge = false;
	}

	public void Kill() {
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
		Die();
	}

	private void Die() {
		BattleContext.EnemiesController.Respawn(this);
	}

	private void OnTriggerEnter(Collider other) { 
		if (other.GetComponent<PlayerShip>() != null) {
			Kill();
		}
    }

	public void UpdateShip() {
		if (!m_inCharge) {
			if (m_rigidbody.velocity.magnitude > 0.5f) {
				m_rigidbody.velocity *= 0.95f;
			}
			Vector3 headingCorrection = headingController.Update(Vector3.Cross(transform.right, BattleContext.PlayerShip.Position - transform.position), Time.fixedDeltaTime);
			m_rigidbody.AddTorque(headingCorrection * 0.2f);
			if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.PlayerShip.Position - Position)) < 10 &&
				m_rigidbody.angularVelocity.magnitude < 0.5f) {
				m_inCharge = true;
			}
		} else {
			m_rigidbody.AddRelativeForce(Vector3.right * 10, ForceMode.Force);
			if (m_rigidbody.velocity.magnitude > 15) {
				m_rigidbody.velocity = m_rigidbody.velocity.normalized * 15;
			}
			if (Mathf.Abs(MathHelper.AngleBetweenVectors(transform.right, BattleContext.PlayerShip.Position - Position)) > 75) {
				m_inCharge = false;
			}
		}

		if (Vector3.Distance(BattleContext.PlayerShip.Position, Position) > 80) {
			Die();
		}
	}


	public Vector3 Position {
		get {
			return transform.position;
		}
	}

}
