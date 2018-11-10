using UnityEngine;
using UnityEngine.Serialization;

public class GUIManagerObsolete : MonoBehaviour {
	[SerializeField]
	private PlayerGUIController _playerGUIController;
	[FormerlySerializedAs("_pauseMenu")]
	[SerializeField]
	private PauseMenuObsolete _pauseMenuObsolete;
	[FormerlySerializedAs("_deathMenu")]
	[SerializeField]
	private DeathMenuObsolete _deathMenuObsolete;

	public void CreateGUI() {
		_playerGUIController.Hide();
		_pauseMenuObsolete.Hide();
		_deathMenuObsolete.Hide();
	}

	public PlayerGUIController PlayerGUIController {
		get {
			return _playerGUIController;
		}
	}

	public PauseMenuObsolete PauseMenuObsolete {
		get {
			return _pauseMenuObsolete;
		}
	}

	public DeathMenuObsolete DeathMenuObsolete {
		get {
			return _deathMenuObsolete;
		}
	}

}
