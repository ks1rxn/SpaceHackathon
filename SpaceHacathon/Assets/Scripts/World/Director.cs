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

		BattleContext.PlayerShip.Initiate();
		BattleContext.BonusesController.Initiate();
		BattleContext.BulletsController.Initiate();
		BattleContext.EnemiesController.Initiate();
		BattleContext.ExplosionsController.Initiate();

		BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
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
		BattleContext.Director.Analytics.LogEvent("BattleScene", "EndBattle", "FPS", Mathf.RoundToInt(m_fps / m_frames));
	}

	public void OnEnemyKill(IEnemyShip enemy) {
		
	}

	public GoogleAnalyticsV4 Analytics {
		get {
			return m_analytics;
		}
	}

}