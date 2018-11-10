using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StateMachine<StatesEnum, EventsEnum> {
        private readonly StatesFactory<StatesEnum, EventsEnum> _stateFactory;
        
        private Stack<IState<StatesEnum, EventsEnum>> _states;
        private Queue<EventsEnum> _events;

        public StateMachine(StatesFactory<StatesEnum, EventsEnum> stateFactory) {
            _stateFactory = stateFactory;
        }
        
        public void Initiate() {
            _states = new Stack<IState<StatesEnum, EventsEnum>>(2);
            _events = new Queue<EventsEnum>(2);
            
            foreach (IState<StatesEnum,EventsEnum> state in _stateFactory.GetAllStates()) {
                state.Initiate();
            }
        }

        public void Start(StatesEnum initialState) {
            ChangeState(initialState);
        }
        
        public void HandleEvent(EventsEnum newEvent) {
            _events.Enqueue(newEvent);
        }
        
        public void Update() {
            if (_states.Count == 0) {
                return;
            }
            while (_events.Count > 0) {
                StateRunResult<StatesEnum> runResult = _states.Peek().HandleEvents(_events.Dequeue());
                if (runResult.StateRunReturnAction != StateRunReturnAction.None) {
                    HandleReturnResult(runResult); 
                    return;
                }
            }
            HandleReturnResult(_states.Peek().Update());
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