using UnityEngine;

public class Director : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerShipPrefab;
	[SerializeField]
	private DifficultyLevel m_difficultyLevel;
	public bool GodMode;

	private PlayerShip m_playerShip;

	private void Awake() {
		//todo: this must be done on LevelLoad scene
		LoadDiffucultySettings((int)m_difficultyLevel);
	}

	private static void LoadDiffucultySettings(int level) {
		TextAsset settingsJson = Resources.Load<TextAsset>("settings/difficulty" + level);
		BattleContext.Settings = JsonUtility.FromJson<LevelSettings>(settingsJson.text);
	}

	private void Start() {
//		Analytics.CustomEvent("gameStart", new Dictionary<string, object> {
//			{ "device", SystemInfo.deviceModel },
//			{ "platform", Application.platform },
//			{ "install", Application.installMode },
//			{ "version", Application.version }
//		});
		BattleContext.Initiate();

		BattleContext.StatisticsManager.Initiate();
		BattleContext.TimeManager.Initiate();
		BattleContext.GUIManager.CreateGUI();

		BattleContext.GUIManager.PlayerGUIController.Show();

		BattleContext.BonusesController.Initiate();
		BattleContext.AlliesController.Initiate();
		BattleContext.EffectsController.Initiate();
		BattleContext.BulletsController.Initiate();
		BattleContext.EnemiesController.Initiate();
		BattleContext.ExplosionsController.Initiate();

		BattleContext.TimeManager.SetTimeScaleMode(TimeScaleMode.Normal);

		SpawnPlayerShip(Vector3.zero, 0);
	}

	public void OnPauseGame() {
		BattleContext.GUIManager.PlayerGUIController.Hide();
		BattleContext.GUIManager.PauseMenu.Show();
		BattleContext.TimeManager.Pause();
		//todo: unscaled time is used in charge process
	}

	public void OnUnpauseGame() {
		BattleContext.GUIManager.PlayerGUIController.Show();
		BattleContext.GUIManager.PauseMenu.Hide();
		BattleContext.TimeManager.Unpause();
	}

	public void OnPlayerDie() {
		BattleContext.GUIManager.PlayerGUIController.Hide();
		BattleContext.GUIManager.DeathMenu.Show();
		BattleContext.TimeManager.Pause();
		BattleContext.StatisticsManager.SendPlayerShipStatistics();
	}

	public void OnTimeExprire() {
		BattleContext.GUIManager.PlayerGUIController.Hide();
		BattleContext.GUIManager.DeathMenu.Show();
		BattleContext.TimeManager.Pause();
		BattleContext.StatisticsManager.SendPlayerShipStatistics();
	}

	private void Update() {
		BattleContext.StatisticsManager.UpdateEntity();
		BattleContext.TimeManager.UpdateEntity();
	}

	private void FixedUpdate() {
		BattleContext.PlayerShip.UpdateEntity();
		BattleContext.BattleCamera.UpdateEntity();
		BattleContext.BulletsController.FixedUpdateEntity();
		BattleContext.BonusesController.FixedUpdateEntity();
		BattleContext.EnemiesController.FixedUpdateEntity();
		BattleContext.AlliesController.FixedUpdateEntity();
		BattleContext.EffectsController.FixedUpdateEntity();
		BattleContext.ExplosionsController.FixedUpdateEntity();
	}

	private void SpawnPlayerShip(Vector3 position, float angle) {
		m_playerShip = (Instantiate(m_playerShipPrefab)).GetComponent<PlayerShip>();
		m_playerShip.transform.parent = transform;
		m_playerShip.Initiate();
		m_playerShip.Spawn(position, angle);
	}

	public PlayerShip PlayerShip {
		get {
			return m_playerShip;
		}
	}

	public Vector3 PlayerPosition {
		get {
			return PlayerShip.Position;
		}
	}

}

public enum DifficultyLevel {
	Easy = 1,
	Medium = 2,
	Hard = 3
}