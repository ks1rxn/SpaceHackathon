using System;
using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public class GameLoop : IInitializable, ITickable, IDisposable, IGameLoop {
        private readonly StateMachine<GameLoopStates, GameLoopEvents> _stateMachine;
        private readonly ElapsedTimeCounter _elapsedTimeCounter;
        
        public GameLoop(StateMachine<GameLoopStates, GameLoopEvents> stateMachine, ElapsedTimeCounter elapsedTimeCounter) {
            _stateMachine = stateMachine;
            _elapsedTimeCounter = elapsedTimeCounter;
        }
        
        public void Initialize() {   
            _stateMachine.Initiate();
            _stateMachine.Start(GameLoopStates.PlayNormal);
            _elapsedTimeCounter.Initiate();
        }

        public void Tick() {
            _stateMachine.Update();
            _elapsedTimeCounter.AddTime(UnityEngine.Time.deltaTime);
        }
        
        public void Dispose() {
            _stateMachine.Dispose();
        }

        public void PushEvent(GameLoopEvents newEvent) {
            _stateMachine.HandleEvent(newEvent);
        }
        
    }

}