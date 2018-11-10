using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StateMachine<StatesEnum, EventsEnum> {
        private readonly StatesFactory<StatesEnum, EventsEnum> _stateFactory;
        
        private Stack<IState<StatesEnum, EventsEnum>> _activeStates;
        private Queue<EventsEnum> _incomingEvents;

        public StateMachine(StatesFactory<StatesEnum, EventsEnum> stateFactory) {
            _stateFactory = stateFactory;
        }
        
        public void Initiate() {
            _activeStates = new Stack<IState<StatesEnum, EventsEnum>>(2);
            _incomingEvents = new Queue<EventsEnum>(2);
            
            foreach (IState<StatesEnum,EventsEnum> state in _stateFactory.GetAllStates()) {
                state.Initiate();
            }
        }

        public void Start(StatesEnum initialState) {
            ChangeState(initialState);
        }
        
        public void HandleEvent(EventsEnum newEvent) {
            _incomingEvents.Enqueue(newEvent);
        }
        
        public void Update() {
            if (_activeStates.Count == 0) {
                return;
            }
            while (_incomingEvents.Count > 0) {
                StateRunResult<StatesEnum> runResult = _activeStates.Peek().HandleEvents(_incomingEvents.Dequeue());
                if (runResult.StateRunReturnAction != StateRunReturnAction.None) {
                    HandleReturnResult(runResult); 
                    return;
                }
            }
            HandleReturnResult(_activeStates.Peek().Update());
        }

        public void Dispose() {
            while (_activeStates.Count > 0) {
                IState<StatesEnum, EventsEnum> state = _activeStates.Pop();
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
            if (_activeStates.Count != 0) {
                IState<StatesEnum, EventsEnum> activeState = _activeStates.Pop();
                activeState.Exit();
            }

            IState<StatesEnum, EventsEnum> enterState = _stateFactory.GetState(newState);
            _activeStates.Push(enterState);
            enterState.Enter();
        }

        private void PushState(StatesEnum newState) {
            if (_activeStates.Count != 0) {
                _activeStates.Peek().Pause();
            }

            IState<StatesEnum, EventsEnum> enterState = _stateFactory.GetState(newState);
            _activeStates.Push(enterState);
            enterState.Enter();
        }

        private void PopState() {
            if (_activeStates.Count != 0) {
                IState<StatesEnum, EventsEnum> activeState = _activeStates.Pop();
                activeState.Exit();
            }

            if (_activeStates.Count != 0) {
                _activeStates.Peek().Resume();
            }
        }
        
    }

}