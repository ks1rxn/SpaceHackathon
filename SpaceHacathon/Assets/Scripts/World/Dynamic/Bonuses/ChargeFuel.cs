using System.Collections.Generic;
using UnityEngine;

public class ChargeFuel : MonoBehaviour {

	public void Spawn(Vector3 position) {
		transform.position = position;
	}

	private void Update() {
		if (Vector3.Distance(BattleContext.PlayerShip.transform.position, transform.position) > 80) {
			Die();
		}
	}

	protected void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<PlayerShip>() != null) {
			Die();
		}
    }

	private void Die() {
		BattleContext.BonusesController.Respawn(this);
	}

}
