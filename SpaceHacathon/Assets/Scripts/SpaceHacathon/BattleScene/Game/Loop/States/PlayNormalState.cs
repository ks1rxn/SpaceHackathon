using System.Collections.Generic;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class PlayNormalState : IState<GameLoopStates, GameLoopEvents> {

        public override void Enter() {
            Debug.Log("Enter PlayNormalState");
        }

        public override void Exit() {
            Debug.Log("Exit PlayNormalState");
        }

        public override StateRunResult<GameLoopStates> HandleEvents(GameLoopEvents nextEvent) {
            switch (nextEvent) {
                case GameLoopEvents.PausePressed:
                    return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.Push, ReturnState = GameLoopStates.InPause};
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

    }

}