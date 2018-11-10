using System;
using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public class GameLoop : IInitializable, ITickable, IDisposable, IGameLoop {
        private readonly StateMachine<GameLoopStates, GameLoopEvents> _stateMachine;
        private readonly ElapsedTimeCounter _elapsedTimeCounter;
        
        private Queue<GameLoopEvents> _events;

        public GameLoop(StateMachine<GameLoopStates, GameLoopEvents> stateMachine, ElapsedTimeCounter elapsedTimeCounter) {
            _stateMachine = stateMachine;
            _elapsedTimeCounter = elapsedTimeCounter;
        }
        
        public void Initialize() {
            _events = new Queue<GameLoopEvents>(2);    
            _stateMachine.Initiate(GameLoopStates.PlayNormal);

            _elapsedTimeCounter.Initiate();
        }

        public void Tick() {
            _stateMachine.Update(_events.Count > 0 ? _events.Dequeue() : GameLoopEvents.None);
            _elapsedTimeCounter.AddTime(UnityEngine.Time.deltaTime);
        }
        
        public void Dispose() {
            _stateMachine.Dispose();
        }

        public void PushEvent(GameLoopEvents newEvent) {
            _events.Enqueue(newEvent);
        }
        
    }

}