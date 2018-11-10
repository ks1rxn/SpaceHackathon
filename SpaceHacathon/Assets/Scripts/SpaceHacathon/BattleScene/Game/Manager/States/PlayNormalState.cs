using SpaceHacathon.Helpers.FSM;

namespace SpaceHacathon.BattleScene.Game.Manager.States {

    public class PlayNormalState : IState<GameManagerStates, GameManagerEvents> {
        
        public override StateRunResult<GameManagerStates> HandleEvents(GameManagerEvents nextEvent) {
            switch (nextEvent) {
                case GameManagerEvents.PausePressed:
                    return new StateRunResult<GameManagerStates>{StateRunReturnAction = StateRunReturnAction.Push, ReturnState = GameManagerStates.InPause};
            }
            return new StateRunResult<GameManagerStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override StateRunResult<GameManagerStates> Update() {
            return new StateRunResult<GameManagerStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override GameManagerStates GetType {
            get {
                return GameManagerStates.PlayNormal;
            }
        }

    }

}