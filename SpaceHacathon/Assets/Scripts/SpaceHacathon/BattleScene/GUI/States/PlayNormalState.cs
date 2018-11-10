using SpaceHacathon.BattleScene.GUI.Screens;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.States {

    public class PlayNormalState : IState<GUIStates, GUIEvents> {
        private readonly ShipGUI _shipGUI;

        public PlayNormalState(ShipGUI shipGUI) {
            _shipGUI = shipGUI;
        }

        public override void Initiate() {
            _shipGUI.Hide();
        }

        public override StateRunResult<GUIStates> Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("To pause");
            }
            return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override GUIStates GetType {
            get {
                return GUIStates.PlayNormal;
            }
        }
        
    }

}