using UnityEngine;

public class GUIManager : MonoBehaviour {
	[SerializeField]
	private GameObject m_playerGUIPrefab;
	[SerializeField]
	private GameObject m_pauseMenuPrefab;

	private PlayerGUIController m_playerGUIController;
	private PauseMenu m_pauseMenu;

	public void CreateGUI() {
		m_playerGUIController = Instantiate(m_playerGUIPrefab).GetComponent<PlayerGUIController>();
		m_playerGUIController.transform.parent = transform;
		m_playerGUIController.Hide();

		m_pauseMenu = Instantiate(m_pauseMenuPrefab).GetComponent<PauseMenu>();
		m_pauseMenu.transform.parent = transform;
		m_pauseMenu.Hide();
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

}
