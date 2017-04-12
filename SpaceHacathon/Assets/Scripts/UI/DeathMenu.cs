using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour {
	[SerializeField]
	private Text m_lastTime;

	public void Show(float timeAlive) {
		gameObject.SetActive(true);
		if (timeAlive <= 0) {
			m_lastTime.gameObject.SetActive(false);
		} else {
			m_lastTime.gameObject.SetActive(true);
			int time = (int) timeAlive;
			int minutes = time / 60;
			int seconds = time % 60;
			m_lastTime.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
		}
	}

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void OnStartEasyClick() {
		BattleContext.NextLevel = DifficultyLevel.Easy;
		SceneManager.LoadScene("BattleScene");
	}

	public void OnStartMediumClick() {
		BattleContext.NextLevel = DifficultyLevel.Medium;
		SceneManager.LoadScene("BattleScene");
	}

	public void OnStartHardClick() {
		BattleContext.NextLevel = DifficultyLevel.Hard;
		SceneManager.LoadScene("BattleScene");
	}

	public void OnExitClick() {
		
	}

}
