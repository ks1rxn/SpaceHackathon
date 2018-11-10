using System;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Manager {

    public class GameManager : IInitializable, ITickable, IDisposable, IEventsReceiver<GameManagerEvents> {
        private readonly StateMachine<GameManagerStates, GameManagerEvents> _stateMachine;
        private readonly ElapsedTimeCounter _elapsedTimeCounter;
        
        public GameManager(StateMachine<GameManagerStates, GameManagerEvents> stateMachine, ElapsedTimeCounter elapsedTimeCounter) {
            _stateMachine = stateMachine;
            _elapsedTimeCounter = elapsedTimeCounter;
        }
        
        public void Initialize() {   
            _stateMachine.Initiate();
            _elapsedTimeCounter.Initiate();
            
            _stateMachine.Start(GameManagerStates.PlayNormal);
        }

        public void Tick() {
            _stateMachine.Update();
            _elapsedTimeCounter.AddTime(UnityEngine.Time.deltaTime);
        }
        
        public void Dispose() {
            _stateMachine.Dispose();
        }

        public void PushEvent(GameManagerEvents newEvent) {
            _stateMachine.HandleEvent(newEvent);
        }
        
    }

}