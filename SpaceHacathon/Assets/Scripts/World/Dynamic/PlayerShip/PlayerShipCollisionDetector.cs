using UnityEngine;

public class PlayerShipCollisionDetector : MonoBehaviour {
    [SerializeField]
    private PlayerShip m_ship;

	private void OnTriggerEnter(Collider other) { 
		if (other.gameObject.GetComponent<ChargeFuel>() != null) {
			m_ship.AddFuel();
		} else if (other.gameObject.GetComponent<Blaster>() != null) {
			m_ship.BlasterHit();
		} else if (other.gameObject.GetComponent<Rocket>() != null) {
			m_ship.RocketHit(other.transform.position);
		} else if (other.gameObject.GetComponent<Laser>() != null) {
			m_ship.LaserHit();
		} else {
			m_ship.EnemyShipHit();
		}
	}

}
