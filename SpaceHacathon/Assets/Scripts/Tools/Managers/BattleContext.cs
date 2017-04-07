using UnityEngine;

public class BattleContext {
	private static BattleManager m_battleManager;
	private static LevelSettings m_settings;

	public static void Initiate() {
		m_battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
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
