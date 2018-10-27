using UnityEngine;

public class GUIManager : MonoBehaviour {
	[SerializeField]
	private PlayerGUIController _playerGUIController;
	[SerializeField]
	private PauseMenu _pauseMenu;
	[SerializeField]
	private DeathMenu _deathMenu;

	public void CreateGUI() {
		_playerGUIController.Hide();
		_pauseMenu.Hide();
		_deathMenu.Hide();
	}

	public PlayerGUIController PlayerGUIController {
		get {
			return _playerGUIController;
		}
	}

	public PauseMenu PauseMenu {
		get {
			return _pauseMenu;
		}
	}

	public DeathMenu DeathMenu {
		get {
			return _deathMenu;
		}
	}

}
