using UnityEngine;

public class BattleContext {
	private static Director m_director;
    private static TimeManager m_timeManager;
	private static PlayerShip m_playerShip;
	private static BattleCamera m_battleCamera;
	private static GUIManager m_guiManager;
	private static ExplosionsController m_explosionsController;
	private static EnemiesController m_enemiesController;
	private static BulletsController m_bulletsController;
	private static BonusesController m_bonusesController;

    public static TimeManager TimeManager {
        get {
			if (m_timeManager == null) {
				m_timeManager = GameObject.Find(EntitiesNames.TimeManager).GetComponent<TimeManager>();
			}
            return m_timeManager;
        }
    }

    public static PlayerShip PlayerShip {
		get {
			if (m_playerShip == null) {
				m_playerShip = GameObject.Find(EntitiesNames.PlayerShip).GetComponent<PlayerShip>();
			}
			return m_playerShip;
		}
	}

	public static BattleCamera BattleCamera {
		get {
			if (m_battleCamera == null) {
				m_battleCamera = GameObject.Find(EntitiesNames.BattleCamera).GetComponent<BattleCamera>();
			}
			return m_battleCamera;
		}
	}

	public static GUIManager GUIManager {
		get {
			if (m_guiManager == null) {
				m_guiManager = GameObject.Find(EntitiesNames.GUIManager).GetComponent<GUIManager>();
			}
			return m_guiManager;
		}
	}

	public static ExplosionsController ExplosionsController {
		get {
			if (m_explosionsController == null) {
				m_explosionsController = GameObject.Find(EntitiesNames.ExplosionsController).GetComponent<ExplosionsController>();
			}
			return m_explosionsController;
		}
	}

	public static EnemiesController EnemiesController {
		get {
			if (m_enemiesController == null) {
				m_enemiesController = GameObject.Find(EntitiesNames.EnemiesController).GetComponent<EnemiesController>();
			}
			return m_enemiesController;
		}
	}

	public static BulletsController BulletsController {
		get {
			if (m_bulletsController == null) {
				m_bulletsController = GameObject.Find(EntitiesNames.BulletsController).GetComponent<BulletsController>();
			}
			return m_bulletsController;
		}
	}

	public static BonusesController BonusesController {
		get {
			if (m_bonusesController == null) {
				m_bonusesController = GameObject.Find(EntitiesNames.BonusesController).GetComponent<BonusesController>();
			}
			return m_bonusesController;
		}
	}

	public static Director Director {
		get {
			if (m_director == null) {
				m_director = GameObject.Find(EntitiesNames.Director).GetComponent<Director>();
			}
			return m_director;
		}
	}

}

class EntitiesNames {
	public const string PlayerShip = "PlayerShip";
	public const string TimeManager = "Dynamic";
	public const string Director = "Dynamic";
	public const string BattleCamera = "Camera";
	public const string GUIManager = "GUIManager";
	public const string ExplosionsController = "ExplosionsController";
	public const string EnemiesController = "EnemiesController";
	public const string BulletsController = "BulletsController";
	public const string BonusesController = "BonusesController";
}