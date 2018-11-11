using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIElements;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens {

    public class ShipGUI : MonoBehaviour {
        [SerializeField]
        private RotationJoystick _rotationJoystick;
        
        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
        
    }

}