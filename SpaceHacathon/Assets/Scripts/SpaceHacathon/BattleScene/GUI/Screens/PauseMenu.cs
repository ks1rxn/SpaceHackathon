using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens {

    public class PauseMenu : MonoBehaviour {
        
        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
        
    }

}