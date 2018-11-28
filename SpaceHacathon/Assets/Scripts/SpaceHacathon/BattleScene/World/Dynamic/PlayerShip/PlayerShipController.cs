using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip {

    public class PlayerShipController : MonoBehaviour {
        private StateMachine<PlayerShipStates, PlayerShipEvents> _stateMachine;

        [Inject]
        public void Construct(StateMachine<PlayerShipStates, PlayerShipEvents> stateMachine) {
            _stateMachine = stateMachine;
        }

        private void Start() {
            _stateMachine.Initiate();
            _stateMachine.Start(PlayerShipStates.FlyingNormalState);
        }
        
        private void FixedUpdate() {
            _stateMachine.Update();
        }

        private void OnDestroy() {
            _stateMachine.Dispose();
        }
        
        public void PushEvent(PlayerShipEvents newEvent) {
            _stateMachine.HandleEvent(newEvent);
        }
        
    }

}