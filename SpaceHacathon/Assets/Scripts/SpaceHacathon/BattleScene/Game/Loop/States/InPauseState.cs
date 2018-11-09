using System.Collections.Generic;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class InPauseState : IState<GameLoopStates> {
        
        public override void Enter() {
            Debug.Log("Enter InPauseState");
        }

        public override void Exit() {
            Debug.Log("Exit InPauseState");
        }
        
        public override StateRunResult<GameLoopStates> HandleEvents(Queue<GameLoopEvent> events) {
            while (events.Count > 0) {
                GameLoopEvent currentEvent = events.Dequeue();
                switch (currentEvent.EventType) {
                    case GameLoopEvent.Type.PausePressed:
                        return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.Pop};
                }
            }
            return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override StateRunResult<GameLoopStates> Update() {
            return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override GameLoopStates GetType {
            get {
                return GameLoopStates.InPause;
            }
        }

        public class Factory : PlaceholderFactory<InPauseState> { }
        
    }

}