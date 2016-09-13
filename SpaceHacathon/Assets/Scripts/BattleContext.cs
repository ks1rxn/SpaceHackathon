using UnityEngine;

public class BattleContext {
	private static PlayerShip m_playerShip;
	private static BattleCamera m_battleCamera;

	public static PlayerShip PlayerShip {
		get {
			return m_playerShip;
		}
		set {
			m_playerShip = value;
		}
	}

	public static BattleCamera BattleCamera {
		get {
			return m_battleCamera;
		}
		set {
			m_battleCamera = value;
		}
	}

}
