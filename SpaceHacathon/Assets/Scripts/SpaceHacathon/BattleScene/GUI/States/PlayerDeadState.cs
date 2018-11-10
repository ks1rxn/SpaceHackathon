using SpaceHacathon.BattleScene.GUI.Screens;
using SpaceHacathon.Helpers.FSM;

namespace SpaceHacathon.BattleScene.GUI.States {

    public class PlayerDeadState : IState<GUIStates, GUIEvents> {
        private readonly DeathMenu _deathMenu;

        public PlayerDeadState(DeathMenu deathMenu) {
            _deathMenu = deathMenu;
        }

        public override void Initiate() {
            _deathMenu.Hide();
        }
        
        public override GUIStates GetType {
            get {
                return GUIStates.PlayerDead;
            }
        }
        
    }

}