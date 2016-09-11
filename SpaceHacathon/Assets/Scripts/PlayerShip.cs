using UnityEngine;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;
	[SerializeField]
	private GameObject m_space;

	protected void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
	}

	protected void Update() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Debug.DrawRay(transform.position, lookVector * 10, Color.red);
		if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("InputPlane"))) {
				float angle = MathHelper.AngleBetweenVectors(lookVector, hit.point - transform.position);
	            if (Mathf.Abs(angle) < 90) {
		            if ((hit.point - transform.position).magnitude > 1.5f) {
			            float length = Mathf.Min((hit.point - transform.position).magnitude, 6);
						m_rigidbody.AddForce(lookVector.normalized * length);
		            }
	            }
	            if (Mathf.Abs(m_rigidbody.angularVelocity.y * 100) < 180) {
					m_rigidbody.AddTorque(new Vector3(0, (angle - m_rigidbody.angularVelocity.y * 50) * 75, 0));
	            }
            }

			if (m_rigidbody.velocity.magnitude > 5) {
				m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
			}
		}

		// Trash //
		Vector3 pos = m_space.transform.position;
		pos.x = transform.position.x;
		pos.z = transform.position.z;
		m_space.transform.position = pos;
		MeshRenderer renderer = m_space.GetComponent<MeshRenderer>();
		renderer.material.SetTextureOffset("_MainTex", new Vector2(-transform.position.x / 1000, -transform.position.z / 1000));
	}

}
