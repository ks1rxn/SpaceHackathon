using UnityEngine;

public class BattleCamera : MonoBehaviour {

	protected void Update() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Vector3 position = BattleContext.PlayerShip.transform.position - lookVector * 7f;
		position.y = 10.5f;
		transform.position = position;
		Vector3 rotation = BattleContext.PlayerShip.transform.eulerAngles;
		rotation.x = 90;
		rotation.y += 90;
		transform.eulerAngles = rotation;
	}

}
