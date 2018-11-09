using System.Collections.Generic;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class PlayNormalState : IState<GameLoopStates> {

        public override void Enter() {
            Debug.Log("Enter PlayNormalState");
        }

        public override void Exit() {
            Debug.Log("Exit PlayNormalState");
        }

        public override StateRunResult<GameLoopStates> HandleEvents(Queue<GameLoopEvent> events) {
            while (events.Count > 0) {
                GameLoopEvent currentEvent = events.Dequeue();
                switch (currentEvent.EventType) {
                    case GameLoopEvent.Type.PausePressed:
                        return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.Push, ReturnState = GameLoopStates.InPause};
                }
            }
            return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override StateRunResult<GameLoopStates> Update() {
            return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override GameLoopStates GetType {
            get {
                return GameLoopStates.PlayNormal;
            }
        }
        
        public class Factory : PlaceholderFactory<PlayNormalState> { }

    }

}