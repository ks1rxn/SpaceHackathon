using UnityEngine;

public class BattleContext {
	private static PlayerShip m_playerShip;

	public static PlayerShip PlayerShip {
		get {
			return m_playerShip;
		}
		set {
			m_playerShip = value;
		}
	}
}
