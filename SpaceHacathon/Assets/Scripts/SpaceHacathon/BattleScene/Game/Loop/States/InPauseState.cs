using SpaceHacathon.Helpers.FSM;
using UnityEngine;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class InPauseState : IState<GameLoopStates, GameLoopEvents> {
        
        public override void Enter() {
            Debug.Log("Enter InPauseState");
        }

        public override void Exit() {
            Debug.Log("Exit InPauseState");
        }
        
        public override StateRunResult<GameLoopStates> HandleEvents(GameLoopEvents nextEvent) {
            switch (nextEvent) {
                case GameLoopEvents.PausePressed:
                    return new StateRunResult<GameLoopStates>{StateRunReturnAction = StateRunReturnAction.Pop};
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

    }

}