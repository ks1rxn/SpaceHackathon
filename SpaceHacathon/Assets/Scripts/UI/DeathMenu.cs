using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour {

	public void Show() {
		gameObject.SetActive(true);
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void OnRestartClick() {
		SceneManager.LoadScene("BattleScene");	
	}

	public void OnExitClick() {
		
	}

}
