using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;

namespace SpaceHacathon.BattleScene.Game.Loop.States {

    public class InPauseState : IState<GameLoopStates, GameLoopEvents> {
        private readonly TimeSpeedController _timeSpeedController;
        
        public InPauseState(TimeSpeedController timeSpeedController) {
            _timeSpeedController = timeSpeedController;
        }
        
        public override void Enter() {
            _timeSpeedController.StopTime();
        }

        public override void Exit() {
            _timeSpeedController.ResumeTime();
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