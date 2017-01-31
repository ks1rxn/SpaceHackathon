using UnityEngine;

public class Director : MonoBehaviour {
	[SerializeField]
	private GoogleAnalyticsV4 m_analytics;

	private float m_fps;
	private int m_frames;

	private void Awake() {
//		Analytics.CustomEvent("gameStart", new Dictionary<string, object> {
//			{ "device", SystemInfo.deviceModel },
//			{ "platform", Application.platform },
//			{ "install", Application.installMode },
//			{ "version", Application.version }
//		});
		BattleContext.TimeManager.Initiate();
		BattleContext.GUIManager.CreateGUI();

		BattleContext.GUIManager.PlayerGUIController.Show();

		BattleContext.PlayerShip.Initiate();
		BattleContext.BonusesController.Initiate();
		BattleContext.BulletsController.Initiate();
		BattleContext.EnemiesController.Initiate();
		BattleContext.ExplosionsController.Initiate();

		BattleContext.TimeManager.SetTimeScaleMode(TimeScaleMode.Normal);
	}

	public void PauseGame() {
		BattleContext.GUIManager.PlayerGUIController.Hide();
		BattleContext.GUIManager.PauseMenu.Show();
		BattleContext.TimeManager.Pause();
		//todo: unscaled time is used in charge process
	}

	public void UnpauseGame() {
		BattleContext.GUIManager.PlayerGUIController.Show();
		BattleContext.GUIManager.PauseMenu.Hide();
		BattleContext.TimeManager.Unpause();
	}

	private void Update() {
		m_fps += Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
		m_frames++;
		BattleContext.TimeManager.UpdateEntity();
	}

	private void FixedUpdate() {
		BattleContext.PlayerShip.UpdateEntity();
		BattleContext.BattleCamera.UpdateEntity();
		BattleContext.BulletsController.UpdateEntity();
		BattleContext.BonusesController.UpdateEntity();
		BattleContext.EnemiesController.UpdateEntity();
	}

	public void OnPlayerDie() {
		BattleContext.GUIManager.PlayerGUIController.Hide();
		BattleContext.GUIManager.DeathMenu.Show();
		BattleContext.TimeManager.Pause();
	}

	public void OnEnemyKill(IEnemyShip enemy) {
		
	}

	public GoogleAnalyticsV4 Analytics {
		get {
			return m_analytics;
		}
	}

}