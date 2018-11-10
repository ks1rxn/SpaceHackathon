using SpaceHacathon.BattleScene.GUI.Screens;
using SpaceHacathon.Helpers.FSM;

namespace SpaceHacathon.BattleScene.GUI.States {

    public class InPauseState : IState<GUIStates, GUIEvents> {
        private readonly PauseMenu _pauseMenu;

        public InPauseState(PauseMenu pauseMenu) {
            _pauseMenu = pauseMenu;
        }

        public override void Initiate() {
            _pauseMenu.Hide();
        }
        
        public override GUIStates GetType {
            get {
                return GUIStates.InPause;
            }
        }

    }

}