using UnityEngine;

public class BattleCamera : MonoBehaviour {
	protected void Awake() {
		BattleContext.BattleCamera = this;
	}

	protected void Update() {
		Vector3 position = BattleContext.PlayerShip.Position;
		position.y = 7.5f;
		position.z -= 9;
		transform.position = position;
		transform.eulerAngles = new Vector3(50, 0, 0);

//		Vector3 position = BattleContext.PlayerShip.transform.position;
//		position.y = 3.5f;
//		position.z -= 4;
//		transform.position = position;
//		transform.eulerAngles = new Vector3(50, 0, 0);
	}

}
