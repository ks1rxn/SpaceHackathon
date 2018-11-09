using System;
using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Loop.States;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public class GameLoop : IInitializable, ITickable, IDisposable {
        private readonly TimeSpeedController _timeSpeedController;
        private readonly StateMachine<GameLoopStates> _stateMachine;
        
        private Queue<GameLoopEvent> _events;

        public GameLoop(TimeSpeedController timeSpeedController, StateMachine<GameLoopStates> stateMachine) {
            _timeSpeedController = timeSpeedController;
            _stateMachine = stateMachine;
        }

        public void Initialize() {
            _events = new Queue<GameLoopEvent>(2);    
            _stateMachine.Initiate(GameLoopStates.PlayNormal);
        }

        public void Tick() {
            _stateMachine.Update(_events);
        }
        
        public void Dispose() {
            _stateMachine.Dispose();
        }

        public void PushEvent(GameLoopEvent newEvent) {
            _events.Enqueue(newEvent);
        }
        
    }

}