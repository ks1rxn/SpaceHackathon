using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.BattleScene.GUI;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;

namespace SpaceHacathon.BattleScene.Game.Manager.States {

    public class InPauseState : IState<GameManagerStates, GameManagerEvents> {
        private readonly IEventsReceiver<GUIEvents> _guiManager;
        private readonly TimeSpeedController _timeSpeedController;
        
        public InPauseState(IEventsReceiver<GUIEvents> guiManager, TimeSpeedController timeSpeedController) {
            _guiManager = guiManager;
            _timeSpeedController = timeSpeedController;
        }
        
        public override void Enter() {
            _timeSpeedController.StopTime();
            _guiManager.PushEvent(GUIEvents.GamePaused);
        }

        public override void Exit() {
            _timeSpeedController.ResumeTime();
            _guiManager.PushEvent(GUIEvents.GameUnpaused);
        }
        
        public override StateRunResult<GameManagerStates> HandleEvents(GameManagerEvents nextEvent) {
            switch (nextEvent) {
                case GameManagerEvents.ResumePressed:
                    return new StateRunResult<GameManagerStates>{StateRunReturnAction = StateRunReturnAction.Pop};
            }
            return new StateRunResult<GameManagerStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override StateRunResult<GameManagerStates> Update() {
            return new StateRunResult<GameManagerStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override GameManagerStates GetType {
            get {
                return GameManagerStates.InPause;
            }
        }

    }

}