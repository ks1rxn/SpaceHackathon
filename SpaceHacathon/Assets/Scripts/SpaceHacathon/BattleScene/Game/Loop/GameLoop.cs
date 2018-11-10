using System;
using System.Collections.Generic;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public interface IGameLoop {
        void PushEvent(GameLoopEvents newEvent);
    }

    public class GameLoop : IInitializable, ITickable, IDisposable, IGameLoop {
        private readonly StateMachine<GameLoopStates, GameLoopEvents> _stateMachine;
        
        private Queue<GameLoopEvents> _events;

        public GameLoop(StateMachine<GameLoopStates, GameLoopEvents> stateMachine) {
            _stateMachine = stateMachine;
        }
        
        public void Initialize() {
            Debug.Log("INIT");
            _events = new Queue<GameLoopEvents>(2);    
            _stateMachine.Initiate(GameLoopStates.PlayNormal);
        }

        public void Tick() {
            _stateMachine.Update(_events.Count > 0 ? _events.Dequeue() : GameLoopEvents.None);
        }
        
        public void Dispose() {
            _stateMachine.Dispose();
        }

        public void PushEvent(GameLoopEvents newEvent) {
            _events.Enqueue(newEvent);
        }
        
    }

}