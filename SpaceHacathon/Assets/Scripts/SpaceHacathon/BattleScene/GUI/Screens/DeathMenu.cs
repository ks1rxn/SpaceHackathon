using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens {

    public class DeathMenu : MonoBehaviour {
        
        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
        
    }

}