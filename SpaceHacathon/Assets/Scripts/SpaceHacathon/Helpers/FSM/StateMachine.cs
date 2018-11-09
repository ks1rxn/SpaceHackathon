using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Loop;

namespace SpaceHacathon.Helpers.FSM {

    public class StateMachine<StatesEnum> {
        private readonly StatesFactory<StatesEnum> _stateFactory;
        
        private Stack<IState<StatesEnum>> _states;

        public StateMachine(StatesFactory<StatesEnum> stateFactory) {
            _stateFactory = stateFactory;
        }
        
        public void Initiate(StatesEnum initialState) {
            _states = new Stack<IState<StatesEnum>>(2);
            ChangeState(initialState);
        }

        public void Update(Queue<GameLoopEvent> events) {
            if (_states.Count != 0) {
                HandleReturnResult(_states.Peek().HandleEvents(events));
                HandleReturnResult(_states.Peek().Update());
            }
        }

        public void Dispose() {
            while (_states.Count > 0) {
                IState<StatesEnum> state = _states.Pop();
                state.Exit();
            }
        }
        
        private void HandleReturnResult(StateRunResult<StatesEnum> runResult) {
            switch (runResult.StateRunReturnAction) {
                case StateRunReturnAction.Change:
                    ChangeState(runResult.ReturnState);
                    break;
                case StateRunReturnAction.Push:
                    PushState(runResult.ReturnState);
                    break;
                case StateRunReturnAction.Pop:
                    PopState();
                    break;
            }
        }  
        
        private void ChangeState(StatesEnum newState) {
            if (_states.Count != 0) {
                IState<StatesEnum> activeState = _states.Pop();
                activeState.Exit();
            }

            IState<StatesEnum> enterState = _stateFactory.GetState(newState);
            _states.Push(enterState);
            enterState.Enter();
        }

        private void PushState(StatesEnum newState) {
            if (_states.Count != 0) {
                _states.Peek().Pause();
            }

            IState<StatesEnum> enterState = _stateFactory.GetState(newState);
            _states.Push(enterState);
            enterState.Enter();
        }

        private void PopState() {
            if (_states.Count != 0) {
                IState<StatesEnum> activeState = _states.Pop();
                activeState.Exit();
            }

            if (_states.Count != 0) {
                _states.Peek().Resume();
            }
        }
        
    }

}