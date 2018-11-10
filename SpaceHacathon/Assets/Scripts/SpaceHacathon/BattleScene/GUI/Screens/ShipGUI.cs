using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens {

    public class ShipGUI : MonoBehaviour {

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
        
    }

}