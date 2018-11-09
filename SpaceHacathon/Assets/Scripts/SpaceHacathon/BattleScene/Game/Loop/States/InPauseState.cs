using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class InPauseState : IState {
        
        public override void Enter() {
            Debug.Log("Enter InPauseState");
        }

        public override void Exit() {
            Debug.Log("Exit InPauseState");
        }
        
        public override GameLoopStateResult HandleEvents(Queue<GameLoopEvent> events) {
            while (events.Count > 0) {
                GameLoopEvent currentEvent = events.Dequeue();
                switch (currentEvent.EventType) {
                    case GameLoopEvent.Type.PausePressed:
                        return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.Pop};
                }
            }
            return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.None};
        }

        public override GameLoopStateResult Update() {
            return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.None};
        }
        
        public class Factory : PlaceholderFactory<InPauseState> { }
        
    }

}