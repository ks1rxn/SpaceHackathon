using UnityEngine;

public class Director : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerShipPrefab;
	[SerializeField]
	private DifficultyLevel m_difficultyLevel;
	public bool GodMode;

	private PlayerShip m_playerShip;

	private bool m_started;

	private void Awake() {
		//todo: this must be done on LevelLoad scene
		if ((int) BattleContext.NextLevel > 0) {
			LoadDiffucultySettings((int)BattleContext.NextLevel);
		}
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
		BattleContext.BattleManager.PrefabsManager.Initiate();
		BattleContext.BattleManager.GUIManager.CreateGUI();

		if (!BattleContext.IsFirstRun) {
			BattleContext.BattleManager.GUIManager.DeathMenu.Show(-1);
			BattleContext.IsFirstRun = true;
			return;
		}

		m_started = true;
		BattleContext.BattleManager.AsteroidField.Initiate();
		BattleContext.BattleManager.TimeManager.Initiate();
		BattleContext.BattleManager.GUIManager.PlayerGUIController.Show();

		foreach (IController controller in BattleContext.BattleManager.Controllers) {
			controller.Initiate();
		}
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
		BattleContext.BattleManager.GUIManager.DeathMenu.Show(BattleContext.BattleManager.TimeManager.GameTime);
		BattleContext.BattleManager.TimeManager.Pause();
		BattleContext.BattleManager.StatisticsManager.SendPlayerShipStatistics();
	}

	private void Update() {
		if (m_started) {
			BattleContext.BattleManager.StatisticsManager.UpdateEntity();
			BattleContext.BattleManager.TimeManager.UpdateEntity();
		}
	}

	private void FixedUpdate() {
		if (m_started) {
			BattleContext.BattleManager.AsteroidField.FixedUpdateEntity();
			BattleContext.BattleManager.Director.PlayerShip.UpdateEntity();
			BattleContext.BattleManager.BattleCamera.UpdateEntity();
			foreach (IController controller in BattleContext.BattleManager.Controllers) {
				controller.FixedUpdateEntity();
			}
		}
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