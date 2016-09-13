using UnityEngine;

public class BattleCamera : MonoBehaviour {
	private int mode = 0;

	protected void Awake() {
		BattleContext.BattleCamera = this;
	}

	protected void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			mode = 0;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			mode = 1;
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			mode = 2;
		}
		switch (mode) {
			case 0:
				CameraStatic();
				break;
			case 1:
				CameraBehind();
				break;
			case 2:
				CameraBehindWide();
				break;
		}
	}

	private void CameraBehind() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Vector3 position = BattleContext.PlayerShip.transform.position - lookVector * 7f;
		position.y = 10.5f;
		transform.position = position;
		Vector3 rotation = BattleContext.PlayerShip.transform.eulerAngles;
		rotation.x = 50;
		rotation.y += 90;
		transform.eulerAngles = rotation;
		GetComponent<Camera>().fieldOfView = 60;
	}

	private void CameraBehindWide() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Vector3 position = BattleContext.PlayerShip.transform.position - lookVector * 5f;
		position.y = 7.5f;
		transform.position = position;
		Vector3 rotation = BattleContext.PlayerShip.transform.eulerAngles;
		rotation.x = 50;
		rotation.y += 90;
		transform.eulerAngles = rotation;
		GetComponent<Camera>().fieldOfView = 75;
	}

	private void CameraStatic() {
		Vector3 position = BattleContext.PlayerShip.transform.position;
		position.y = 7.5f;
		position.z -= 7;
		transform.position = position;
		transform.eulerAngles = new Vector3(50, 0, 0);
		GetComponent<Camera>().fieldOfView = 75;
	}

	public int Mode {
		get {
			return mode;
		}
	}
}
