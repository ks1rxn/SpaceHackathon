using UnityEngine;

public class BattleCamera : MonoBehaviour {

	protected void Update() {
		Vector3 lookVector = new Vector3(Mathf.Cos(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-BattleContext.PlayerShip.transform.rotation.eulerAngles.y * Mathf.PI / 180));
		Vector3 position = BattleContext.PlayerShip.transform.position - lookVector * 5f;
		position.y = 7.5f;
		transform.position = position;
		Vector3 rotation = BattleContext.PlayerShip.transform.eulerAngles;
		rotation.x = 60;
		rotation.y += 90;
		transform.eulerAngles = rotation;
	}

}
