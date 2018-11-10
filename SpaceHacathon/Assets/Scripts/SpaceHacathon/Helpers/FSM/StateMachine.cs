using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StateMachine<StatesEnum, EventsEnum> {
        private readonly StatesFactory<StatesEnum, EventsEnum> _stateFactory;
        
        private Stack<IState<StatesEnum, EventsEnum>> _states;

        public StateMachine(StatesFactory<StatesEnum, EventsEnum> stateFactory) {
            _stateFactory = stateFactory;
        }
        
        public void Initiate(StatesEnum initialState) {
            _states = new Stack<IState<StatesEnum, EventsEnum>>(2);
            ChangeState(initialState);
        }

        public void Update(EventsEnum nextEvent) {
            if (_states.Count != 0) {
                HandleReturnResult(_states.Peek().HandleEvents(nextEvent));
                HandleReturnResult(_states.Peek().Update());
            }
        }

        public void Dispose() {
            while (_states.Count > 0) {
                IState<StatesEnum, EventsEnum> state = _states.Pop();
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
                IState<StatesEnum, EventsEnum> activeState = _states.Pop();
                activeState.Exit();
            }

            IState<StatesEnum, EventsEnum> enterState = _stateFactory.GetState(newState);
            _states.Push(enterState);
            enterState.Enter();
        }

        private void PushState(StatesEnum newState) {
            if (_states.Count != 0) {
                _states.Peek().Pause();
            }

            IState<StatesEnum, EventsEnum> enterState = _stateFactory.GetState(newState);
            _states.Push(enterState);
            enterState.Enter();
        }

        private void PopState() {
            if (_states.Count != 0) {
                IState<StatesEnum, EventsEnum> activeState = _states.Pop();
                activeState.Exit();
            }

            if (_states.Count != 0) {
                _states.Peek().Resume();
            }
        }
        
    }

}