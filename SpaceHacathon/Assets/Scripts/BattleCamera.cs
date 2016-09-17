using UnityEngine;

public class BattleCamera : MonoBehaviour {
	protected void Awake() {
		BattleContext.BattleCamera = this;
	}

	protected void Update() {
		Vector3 position = BattleContext.PlayerShip.transform.position;
		position.y = 6.5f;
		position.z -= 6;
		transform.position = position;
		transform.eulerAngles = new Vector3(50, 0, 0);
		GetComponent<Camera>().fieldOfView = 75;
	}

}
