using SpaceHacathon.BattleScene.GUI.Screens;
using SpaceHacathon.Helpers.FSM;

namespace SpaceHacathon.BattleScene.GUI.States {

    public class DeathMenuState : IState<GUIStates, GUIEvents> {
        private readonly DeathMenu _deathMenu;

        public DeathMenuState(DeathMenu deathMenu) {
            _deathMenu = deathMenu;
        }

        public override void Initiate() {
            _deathMenu.Hide();
        }
        
        public override GUIStates GetType {
            get {
                return GUIStates.DeathMenu;
            }
        }
        
    }

}