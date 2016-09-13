using UnityEngine;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	[SerializeField]
	private GameObject m_space;

	protected void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(-1, 0, 0);
	}

	protected void Update() {
		if (BattleContext.BattleCamera.Mode == 0) {
			MoveForStatic();
		} else {
			MoveForBehind();
		}

		// Trash //
		Vector3 pos = m_space.transform.position;
		pos.x = transform.position.x;
		pos.z = transform.position.z;
		m_space.transform.position = pos;
		MeshRenderer spaceRenderer = m_space.GetComponent<MeshRenderer>();
		spaceRenderer.material.SetTextureOffset("_MainTex", new Vector2(-transform.position.x / 1000, -transform.position.z / 1000));
	}

	private void MoveForBehind() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		bool leftEngine = false;
		bool rightEngine = false;
		if (Input.GetKey(KeyCode.D)) {
			leftEngine = true;
		}
		if (Input.GetKey(KeyCode.A)) {
			rightEngine = true;
		}
		if ((leftEngine) && (!rightEngine)) {
			if (m_rigidbody.angularVelocity.magnitude < 0.75) {
				m_rigidbody.AddTorque(new Vector3(0, 3000, 0));
			}
//			m_rigidbody.AddForce(lookVector);
		}
		if ((!leftEngine) && (rightEngine)) {
//			m_rigidbody.AddForce(lookVector);
			if (m_rigidbody.angularVelocity.magnitude < 0.75) {
				m_rigidbody.AddTorque(new Vector3(0, -3000, 0));
			}
		}
		if ((leftEngine) && (rightEngine)) {
			m_rigidbody.AddForce(lookVector * 6);
		}
		if (m_rigidbody.velocity.magnitude > 5) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
		}
	}

	private void MoveForStatic() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Debug.DrawRay(transform.position, lookVector * 10, Color.red);
		if (Input.GetKeyDown(KeyCode.Space)) {
			m_rigidbody.AddExplosionForce(100, transform.position - lookVector.normalized * 10, 50);
		} else if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("InputPlane"))) {
				float angle = MathHelper.AngleBetweenVectors(lookVector, hit.point - transform.position);
	            if (Mathf.Abs(angle) < 90) {
		            if ((hit.point - transform.position).magnitude > 0.5f) {
			            float length = Mathf.Min((hit.point - transform.position).magnitude, 6);
						m_rigidbody.AddForce(lookVector.normalized * length * 2);
		            }
	            }
				m_rigidbody.AddTorque(new Vector3(0, (angle - m_rigidbody.angularVelocity.y * 50) * 75, 0));
	            if (m_rigidbody.angularVelocity.magnitude > 2.8f) {
		            m_rigidbody.angularVelocity = m_rigidbody.angularVelocity.normalized * 2.8f;
	            }
            }

			if (m_rigidbody.velocity.magnitude > 5) {
				m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
			}
		}
	}

}
