using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StateMachine<TStatesEnum, TEventsEnum> {
        private readonly StatesFactory<TStatesEnum, TEventsEnum> _stateFactory;
        
        private Stack<IState<TStatesEnum, TEventsEnum>> _activeStates;
        private Queue<TEventsEnum> _incomingEvents;

        public StateMachine(StatesFactory<TStatesEnum, TEventsEnum> stateFactory) {
            _stateFactory = stateFactory;
        }
        
        public void Initiate() {
            _activeStates = new Stack<IState<TStatesEnum, TEventsEnum>>(2);
            _incomingEvents = new Queue<TEventsEnum>(2);
            
            foreach (IState<TStatesEnum,TEventsEnum> state in _stateFactory.GetAllStates()) {
                state.Initiate();
            }
        }

        public void Start(TStatesEnum initialState) {
            ChangeState(initialState);
        }
        
        public void HandleEvent(TEventsEnum newEvent) {
            _incomingEvents.Enqueue(newEvent);
        }
        
        public void Update() {
            if (_activeStates.Count == 0) {
                return;
            }
            while (_incomingEvents.Count > 0) {
                StateRunResult<TStatesEnum> runResult = _activeStates.Peek().HandleEvents(_incomingEvents.Dequeue());
                if (runResult.StateRunReturnAction != StateRunReturnAction.None) {
                    HandleReturnResult(runResult); 
                    return;
                }
            }
            HandleReturnResult(_activeStates.Peek().Update());
        }

        public void Dispose() {
            while (_activeStates.Count > 0) {
                IState<TStatesEnum, TEventsEnum> state = _activeStates.Pop();
                state.Exit();
            }
        }
        
        private void HandleReturnResult(StateRunResult<TStatesEnum> runResult) {
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
        
        private void ChangeState(TStatesEnum newState) {
            if (_activeStates.Count != 0) {
                IState<TStatesEnum, TEventsEnum> activeState = _activeStates.Pop();
                activeState.Exit();
            }

            IState<TStatesEnum, TEventsEnum> enterState = _stateFactory.GetState(newState);
            _activeStates.Push(enterState);
            enterState.Enter();
        }

        private void PushState(TStatesEnum newState) {
            if (_activeStates.Count != 0) {
                _activeStates.Peek().Pause();
            }

            IState<TStatesEnum, TEventsEnum> enterState = _stateFactory.GetState(newState);
            _activeStates.Push(enterState);
            enterState.Enter();
        }

        private void PopState() {
            if (_activeStates.Count != 0) {
                IState<TStatesEnum, TEventsEnum> activeState = _activeStates.Pop();
                activeState.Exit();
            }

            if (_activeStates.Count != 0) {
                _activeStates.Peek().Resume();
            }
        }
        
    }

}