using SpaceHacathon.BattleScene.Game.Manager;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.GUI {

    public class GUIManager : MonoBehaviour, IEventsReceiver<GUIEvents> {
        private StateMachine<GUIStates, GUIEvents> _stateMachine;

        [Inject]
        private void Construct(StateMachine<GUIStates, GUIEvents> stateMachine) {
            _stateMachine = stateMachine;
        }

        //todo: bad idea: inject PlayerShip into GUI layer. Use abstraction IControllable instead?
        private void Start() {
            _stateMachine.Initiate();
            //todo: start should be initiated from GameManager via event, signal or smthng like this.
            _stateMachine.Start(GUIStates.ShipGUI);
        }

        private void Update() {
            _stateMachine.Update();
        }

        private void OnDestroy() {
            _stateMachine.Dispose();
        }

        public void PushEvent(GUIEvents newEvent) {
            _stateMachine.HandleEvent(newEvent);
        }
        
    }

}