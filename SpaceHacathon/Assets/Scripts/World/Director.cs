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
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;

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
		Time.timeScale = 0;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		//todo: unscaled time is used in charge process
	}

	public void UnpauseGame() {
		BattleContext.GUIManager.PlayerGUIController.Show();
		BattleContext.GUIManager.PauseMenu.Hide();
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

	private void Update() {
		m_fps += Mathf.RoundToInt(1 / Time.deltaTime * Time.timeScale);
		m_frames++;
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
		Time.timeScale = 0;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
	}

	public void OnEnemyKill(IEnemyShip enemy) {
		
	}

	public GoogleAnalyticsV4 Analytics {
		get {
			return m_analytics;
		}
	}

}