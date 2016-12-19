using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour {
	private Rigidbody m_rigidbody;
	private float m_lifeTime;
	private float m_detonatorActivateTime;

	[SerializeField]
	private ParticleSystem m_trail;

	private bool m_inactive;

	private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

	protected void Awake() {
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Spawn(Vector3 position) {
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		m_trail.Stop();
		gameObject.SetActive(true);
	
		transform.position = position;
		m_lifeTime = 10;

		transform.rotation = new Quaternion();;
		
		StartCoroutine(LaunchProcess());
	}

	private IEnumerator LaunchProcess() {
		m_inactive = true;
		m_rigidbody.drag = 3.0f;
		GetComponent<Collider>().enabled = false;

		m_rigidbody.AddExplosionForce(160 * m_rigidbody.mass, transform.position + new Vector3(0, -0.04f, 0), 1);
		yield return new WaitForSeconds(1.0f);

		m_trail.Play();

		GetComponent<Collider>().enabled = true;
		m_rigidbody.drag = 0.0f;
		m_inactive = false;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<PlayerShipHull>() != null || other.GetComponent<ChargeFuel>() != null || other.GetComponent<Rocket>() != null) {
			BattleContext.ExplosionsController.RocketExplosion(transform.position);
			Die();
		}
	}

	public void UpdateBullet() {
		if (m_inactive) {
			return;
		}

		Vector3 enemyPosition = BattleContext.PlayerShip.Position;

		Vector3 angularVelocityError = m_rigidbody.angularVelocity * -1;
        Debug.DrawRay(transform.position, m_rigidbody.angularVelocity * 10, Color.red);
         
        Vector3 angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.fixedDeltaTime);
        Debug.DrawRay(transform.position, angularVelocityCorrection, Color.green);
 
        m_rigidbody.AddTorque(angularVelocityCorrection * 1.0f);
 
        Vector3 desiredHeading = enemyPosition - transform.position;
        Debug.DrawRay(transform.position, desiredHeading, Color.magenta);
 
        Vector3 currentHeading = transform.up;
        Debug.DrawRay(transform.position, currentHeading * 15, Color.blue);
 
        Vector3 headingError = Vector3.Cross(currentHeading, desiredHeading);
        Vector3 headingCorrection = headingController.Update(headingError, Time.fixedDeltaTime);
 
        m_rigidbody.AddTorque(headingCorrection * 1.2f);

		m_rigidbody.AddRelativeForce(0, 120, 0);
		if (m_rigidbody.velocity.magnitude > 6) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 6;
		}

		Vector3 pos = transform.position;
		if (pos.y > 0.1f) {
			pos.y -= Time.fixedDeltaTime;
		} else if (pos.y < -0.1f) {
			pos.y += Time.fixedDeltaTime;
		}
		transform.position = pos;

		m_lifeTime -= Time.fixedDeltaTime;
		if (m_lifeTime <= 0) {
			Die();
			BattleContext.ExplosionsController.RocketExplosion(transform.position);
		}

		float distToPlayer = Vector3.Distance(BattleContext.PlayerShip.Position, transform.position);
		if (distToPlayer > 25) {
			Die();
		}
	}

	private void Die() {
		m_trail.Stop();
		gameObject.SetActive(false);
	}

}
