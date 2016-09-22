using UnityEngine;

public class BattleCamera : MonoBehaviour {
	protected void Awake() {
		BattleContext.BattleCamera = this;
	}

	protected void Update() {
		Vector3 position = BattleContext.PlayerShip.transform.position;
		position.y = 7.5f;
		position.z -= 9;
		Vector3 camPosition = transform.position;
		camPosition.y = 7.5f;
		Vector3 moveVector = position - camPosition;
//		moveVector.y = 7.5f;
//		transform.position += moveVector * Time.deltaTime * 5;
		transform.position = position;
//		Rigidbody body = GetComponent<Rigidbody>();
//		body.AddForce(moveVector * Time.deltaTime * 400);
//		if (body.velocity.magnitude > 20) {
//			body.velocity = body.velocity.normalized * 20;
//		}

		transform.eulerAngles = new Vector3(50, 0, 0);
	}

}
