using UnityEngine;

public class Blaster : MonoBehaviour {
	private float m_angle;

	public void Spawn(Vector3 position, float angle) {
		m_angle = angle;
		transform.position = position;
		transform.rotation = new Quaternion();
		transform.Rotate(new Vector3(0, 1, 0), m_angle);
	}

	protected void Update() {
		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * Time.deltaTime * 20;
	}

}
