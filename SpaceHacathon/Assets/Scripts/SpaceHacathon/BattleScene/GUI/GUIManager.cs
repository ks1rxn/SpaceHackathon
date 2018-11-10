using SpaceHacathon.BattleScene.Game.Loop;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.GUI {

    public class GUIManager : MonoBehaviour {
        private IGameLoop _gameLoop;
        private StateMachine<GUIStates, GUIEvents> _stateMachine;

        [Inject]
        private void Construct(IGameLoop gameLoop, StateMachine<GUIStates, GUIEvents> stateMachine) {
            _gameLoop = gameLoop;
            _stateMachine = stateMachine;
        }

        //todo: bad idea: inject PlayerShip into GUI layer. Use abstraction IControllable instead?
        //todo: use abstraction in GameController too??
        private void Start() {
            _stateMachine.Initiate();
            _stateMachine.Start(GUIStates.PlayNormal);
        }

        private void Update() {
            _stateMachine.Update();
        }

        private void OnDestroy() {
            _stateMachine.Dispose();
        }

    }

}