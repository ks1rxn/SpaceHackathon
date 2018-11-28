using SpaceHacathon.Helpers.FSM;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.States {

    public class DeadState : IState<PlayerShipStates, PlayerShipEvents> {

        public override PlayerShipStates GetType {
            get {
                return PlayerShipStates.DeadState;
            }
        }
        
    }

}