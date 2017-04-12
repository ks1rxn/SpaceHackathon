using UnityEngine;

public class BattleContext {
	private static BattleManager m_battleManager;
	private static LevelSettings m_settings;
	private static DifficultyLevel m_nextLevel;
	private static bool m_isFirstRun;

	public static void Initiate() {
		m_battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
	}

	public static bool IsFirstRun {
		get {
			return m_isFirstRun;
		}
		set {
			m_isFirstRun = value;
		}
	}

	public static DifficultyLevel NextLevel {
		get {
			return m_nextLevel;
		}
		set {
			m_nextLevel = value;
		}
	}

	public static BattleManager BattleManager {
		get {
			return m_battleManager;
		}
	}

	public static LevelSettings Settings {
		get {
			return m_settings;
		}
		set {
			m_settings = value;
		}
	}

}
