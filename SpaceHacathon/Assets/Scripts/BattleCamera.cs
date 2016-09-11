using UnityEngine;

public class BattleCamera : MonoBehaviour {

	protected void Update() {
		Vector3 position = BattleContext.PlayerShip.transform.position;;
		position.y = 15;
		transform.position = position;
	}

}
