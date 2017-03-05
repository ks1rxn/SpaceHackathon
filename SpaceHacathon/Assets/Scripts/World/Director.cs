using UnityEngine;

public class Director : MonoBehaviour {
	[SerializeField]
	private Vector3 m_playerShipPosition;
	[SerializeField]
	private float m_playerShipAngle;
	[SerializeField]
	private GameObject m_playerShipPrefab;
	private PlayerShip m_playerShip;
	
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

		SpawnPlayerShip(m_playerShipPosition, m_playerShipAngle);
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
		BattleContext.BulletsController.UpdateEntity();
		BattleContext.BonusesController.UpdateEntity();
		BattleContext.EnemiesController.UpdateEntity();
		BattleContext.AlliesController.UpdateEntity();
		BattleContext.EffectsController.UpdateEntity();
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