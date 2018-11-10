using SpaceHacathon.BattleScene.Game.Manager;
using SpaceHacathon.BattleScene.GUI.Screens;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.States {

    public class PauseMenuState : IState<GUIStates, GUIEvents> {
        private readonly IEventsReceiver<GameManagerEvents> _gameManager;
        private readonly PauseMenu _pauseMenu;

        public PauseMenuState(IEventsReceiver<GameManagerEvents> gameManager, PauseMenu pauseMenu) {
            _gameManager = gameManager;
            _pauseMenu = pauseMenu;
        }

        public override void Initiate() {
            _pauseMenu.Hide();
        }

        public override void Enter() {
            _pauseMenu.Show();
        }

        public override void Exit() {
            _pauseMenu.Hide();
        }

        public override void Pause() {
            _pauseMenu.Hide();
        }

        public override void Resume() {
            _pauseMenu.Show();
        }
        
        public override StateRunResult<GUIStates> HandleEvents(GUIEvents nextEvent) {
            switch (nextEvent) {
                case GUIEvents.GameUnpaused:
                    return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.Pop};
            }
            return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override StateRunResult<GUIStates> Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _gameManager.PushEvent(GameManagerEvents.ResumePressed);
            }
            return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.None};
        }
        
        public override GUIStates GetType {
            get {
                return GUIStates.PauseMenu;
            }
        }

    }

}