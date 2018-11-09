using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class PlayNormalState : IState {

        public override void Enter() {
            Debug.Log("Enter PlayNormalState");
        }

        public override void Exit() {
            Debug.Log("Exit PlayNormalState");
        }

        public override GameLoopStateResult HandleEvents(Queue<GameLoopEvent> events) {
            while (events.Count > 0) {
                GameLoopEvent currentEvent = events.Dequeue();
                switch (currentEvent.EventType) {
                    case GameLoopEvent.Type.PausePressed:
                        return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.Push, ReturnState = GameLoopStates.InPause};
                }
            }
            return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.None};
        }

        public override GameLoopStateResult Update() {
            return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.None};
        }

        public class Factory : PlaceholderFactory<PlayNormalState> { }

    }

}