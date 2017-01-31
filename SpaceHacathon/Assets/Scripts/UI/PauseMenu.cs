using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	protected void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			BattleContext.Director.OnUnpauseGame();
		}
	}

	public void OnResumeClick() {
		BattleContext.Director.OnUnpauseGame();
	}

	public void OnRestartClick() {
		SceneManager.LoadScene("BattleScene");	
	}

	public void OnExitClick() {
		
	}

}
