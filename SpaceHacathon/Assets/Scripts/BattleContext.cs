﻿using UnityEngine;

public class BattleContext {
    private static World m_world;
	private static PlayerShip m_playerShip;
	private static BattleCamera m_battleCamera;
	private static GUIController m_guiController;
	private static ExplosionsController m_explosionsController;
	private static EnemiesController m_enemiesController;
	private static BulletsController m_bulletsController;

    public static World World {
        get {
            return m_world;
        }
        set {
            m_world = value;
        }
    }

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

	public static EnemiesController EnemiesController {
		get {
			return m_enemiesController;
		}
		set {
			m_enemiesController = value;
		}
	}

	public static BulletsController BulletsController {
		get {
			return m_bulletsController;
		}
		set {
			m_bulletsController = value;
		}
	}

}
