﻿using UnityEngine;

public class BattleContext {
	private static PlayerShip m_playerShip;
	private static BattleCamera m_battleCamera;
	private static GUIController m_guiController;
	private static ExplosionsController m_explosionsController;

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

	public static GUIController GUIController {
		get {
			return m_guiController;
		}
		set {
			m_guiController = value;
		}
	}

	public static ExplosionsController ExplosionsController {
		get {
			return m_explosionsController;
		}
		set {
			m_explosionsController = value;
		}
	}
}
