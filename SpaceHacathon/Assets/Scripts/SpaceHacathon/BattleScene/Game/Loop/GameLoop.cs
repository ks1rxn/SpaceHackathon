using System;
using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Time;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public class GameLoop : IInitializable, ITickable, IDisposable {
        private readonly TimeSpeedController _timeSpeedController;
        private readonly StatesFactory _stateFactory;
        
        private Queue<GameLoopEvent> _events;
        private Stack<IState> _states;

        public GameLoop(TimeSpeedController timeSpeedController, StatesFactory stateFactory) {
            _timeSpeedController = timeSpeedController;
            _stateFactory = stateFactory;
        }

        public void Initialize() {
            _events = new Queue<GameLoopEvent>(2);
            _states = new Stack<IState>(2);
            
            ChangeState(GameLoopStates.PlayNormal);
        }

        public void Tick() {
            if (_states.Count != 0) {
                HandleReturnResult(_states.Peek().HandleEvents(_events));
                HandleReturnResult(_states.Peek().Update());
            }
        }

        private void HandleReturnResult(GameLoopStateResult result) {
            switch (result.ReturnAction) {
                case GameLoopStateResult.Action.Change:
                    ChangeState(result.ReturnState);
                    break;
                case GameLoopStateResult.Action.Push:
                    PushState(result.ReturnState);
                    break;
                case GameLoopStateResult.Action.Pop:
                    PopState();
                    break;
            }
        }
        
        public void Dispose() {
            while (_states.Count > 0) {
                IState state = _states.Pop();
                state.Exit();
            }
        }

        public void PushEvent(GameLoopEvent newEvent) {
            _events.Enqueue(newEvent);
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