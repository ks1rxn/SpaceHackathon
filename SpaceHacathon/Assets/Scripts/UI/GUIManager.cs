using UnityEngine;

public class GUIManager : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerGUIPrefab;
	[SerializeField]
	private GameObject m_pauseMenuPrefab;
	[SerializeField]
	private GameObject m_deathMenuPrefab;

	private PlayerGUIController m_playerGUIController;
	private PauseMenu m_pauseMenu;
	private DeathMenu m_deathMenu;

	public void CreateGUI() {
		m_playerGUIController = Instantiate(m_playerGUIPrefab).GetComponent<PlayerGUIController>();
		m_playerGUIController.transform.SetParent(transform, false);
		m_playerGUIController.Hide();

		m_pauseMenu = Instantiate(m_pauseMenuPrefab).GetComponent<PauseMenu>();
		m_pauseMenu.transform.SetParent(transform, false);
		m_pauseMenu.Hide();

		m_deathMenu = Instantiate(m_deathMenuPrefab).GetComponent<DeathMenu>();
		m_deathMenu.transform.SetParent(transform, false);
		m_deathMenu.Hide();
	}

	public PlayerGUIController PlayerGUIController {
		get {
			return m_playerGUIController;
		}
	}

	public PauseMenu PauseMenu {
		get {
			return m_pauseMenu;
		}
	}

	public DeathMenu DeathMenu {
		get {
			return m_deathMenu;
		}
	}

}
