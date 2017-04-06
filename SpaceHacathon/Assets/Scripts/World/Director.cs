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

		BattleContext.BattleManager.StatisticsManager.Initiate();
		BattleContext.BattleManager.TimeManager.Initiate();
		BattleContext.BattleManager.GUIManager.CreateGUI();

		BattleContext.BattleManager.GUIManager.PlayerGUIController.Show();

		BattleContext.BattleManager.BonusesController.Initiate();
		BattleContext.BattleManager.AlliesController.Initiate();
		BattleContext.BattleManager.EffectsController.Initiate();
		BattleContext.BattleManager.BulletsController.Initiate();
		BattleContext.BattleManager.EnemiesController.Initiate();
		BattleContext.BattleManager.ExplosionsController.Initiate();

		BattleContext.BattleManager.TimeManager.SetTimeScaleMode(TimeScaleMode.Normal);

		SpawnPlayerShip(Vector3.zero, 0);
	}

	public void OnPauseGame() {
		BattleContext.BattleManager.GUIManager.PlayerGUIController.Hide();
		BattleContext.BattleManager.GUIManager.PauseMenu.Show();
		BattleContext.BattleManager.TimeManager.Pause();
		//todo: unscaled time is used in charge process
	}

	public void OnUnpauseGame() {
		BattleContext.BattleManager.GUIManager.PlayerGUIController.Show();
		BattleContext.BattleManager.GUIManager.PauseMenu.Hide();
		BattleContext.BattleManager.TimeManager.Unpause();
	}

	public void OnPlayerDie() {
		BattleContext.BattleManager.GUIManager.PlayerGUIController.Hide();
		BattleContext.BattleManager.GUIManager.DeathMenu.Show();
		BattleContext.BattleManager.TimeManager.Pause();
		BattleContext.BattleManager.StatisticsManager.SendPlayerShipStatistics();
	}

	private void Update() {
		BattleContext.BattleManager.StatisticsManager.UpdateEntity();
		BattleContext.BattleManager.TimeManager.UpdateEntity();
	}

	private void FixedUpdate() {
		BattleContext.BattleManager.Director.PlayerShip.UpdateEntity();
		BattleContext.BattleManager.BattleCamera.UpdateEntity();
		BattleContext.BattleManager.BulletsController.FixedUpdateEntity();
		BattleContext.BattleManager.BonusesController.FixedUpdateEntity();
		BattleContext.BattleManager.EnemiesController.FixedUpdateEntity();
		BattleContext.BattleManager.AlliesController.FixedUpdateEntity();
		BattleContext.BattleManager.EffectsController.FixedUpdateEntity();
		BattleContext.BattleManager.ExplosionsController.FixedUpdateEntity();
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

}

public enum DifficultyLevel {
	Easy = 1,
	Medium = 2,
	Hard = 3
}