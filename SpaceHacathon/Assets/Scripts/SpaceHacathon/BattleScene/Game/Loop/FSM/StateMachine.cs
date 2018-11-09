using System.Collections.Generic;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public class StateMachine {
        private readonly StatesFactory _stateFactory;
        
        private Stack<IState> _states;

        public StateMachine(StatesFactory stateFactory) {
            _stateFactory = stateFactory;
        }
        
        public void Initiate() {
            _states = new Stack<IState>(2);
        }

        public void Update() {
            
        }

        public void Dispose() {
            
        }
        
        private void ChangeState(GameLoopStates newState) {
            if (_states.Count != 0) {
                IState activeState = _states.Pop();
                activeState.Exit();
            }

            IState enterState = _stateFactory.GetState(newState);
            _states.Push(enterState);
            enterState.Enter();
        }

        private void PushState(GameLoopStates newState) {
            if (_states.Count != 0) {
                _states.Peek().Pause();
            }

            IState enterState = _stateFactory.GetState(newState);
            _states.Push(enterState);
            enterState.Enter();
        }

        private void PopState() {
            if (_states.Count != 0) {
                IState activeState = _states.Pop();
                activeState.Exit();
            }

            if (_states.Count != 0) {
                _states.Peek().Resume();
            }
        }
        
    }

}