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
	private static StatisticsManager m_statisticsManager;

	public static void Initiate() {
		m_timeManager = GameObject.Find(EntitiesNames.TimeManager).GetComponent<TimeManager>();
		m_playerShip = GameObject.Find(EntitiesNames.PlayerShip).GetComponent<PlayerShip>();
		m_battleCamera = GameObject.Find(EntitiesNames.BattleCamera).GetComponent<BattleCamera>();
		m_guiManager = GameObject.Find(EntitiesNames.GUIManager).GetComponent<GUIManager>();
		m_explosionsController = GameObject.Find(EntitiesNames.ExplosionsController).GetComponent<ExplosionsController>();
		m_enemiesController = GameObject.Find(EntitiesNames.EnemiesController).GetComponent<EnemiesController>();
		m_bulletsController = GameObject.Find(EntitiesNames.BulletsController).GetComponent<BulletsController>();
		m_bonusesController = GameObject.Find(EntitiesNames.BonusesController).GetComponent<BonusesController>();
		m_director = GameObject.Find(EntitiesNames.Director).GetComponent<Director>();
		m_statisticsManager = GameObject.Find(EntitiesNames.StatisticsManager).GetComponent<StatisticsManager>();
	}

	public static Director Director {
		get {
			return m_director;
		}
	}

	public static TimeManager TimeManager {
		get {
			return m_timeManager;
		}
	}

	public static PlayerShip PlayerShip {
		get {
			return m_playerShip;
		}
	}

	public static BattleCamera BattleCamera {
		get {
			return m_battleCamera;
		}
	}

	public static GUIManager GUIManager {
		get {
			return m_guiManager;
		}
	}

	public static ExplosionsController ExplosionsController {
		get {
			return m_explosionsController;
		}
	}

	public static EnemiesController EnemiesController {
		get {
			return m_enemiesController;
		}
	}

	public static BulletsController BulletsController {
		get {
			return m_bulletsController;
		}
	}

	public static BonusesController BonusesController {
		get {
			return m_bonusesController;
		}
	}

	public static StatisticsManager StatisticsManager {
		get {
			return m_statisticsManager;
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
	public const string StatisticsManager = "Dynamic";
}