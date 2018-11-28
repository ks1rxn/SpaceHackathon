using System.Collections.Generic;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.States {

    public class FlyingNormalState : IState<PlayerShipStates, PlayerShipEvents> {
        private IList<IBehaviour> _behaviours;

        [Inject]
        public void Construct(List<IBehaviour> behaviours) {
            _behaviours = behaviours;
        }
        
        
        public override StateRunResult<PlayerShipStates> Update() {
            foreach (IBehaviour behaviour in _behaviours) {
                behaviour.Run();
            }
            return new StateRunResult<PlayerShipStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override PlayerShipStates GetType => PlayerShipStates.FlyingNormalState;

    }

}