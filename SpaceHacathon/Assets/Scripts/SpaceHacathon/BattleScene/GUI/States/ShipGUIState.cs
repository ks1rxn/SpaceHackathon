using SpaceHacathon.BattleScene.Game.Manager;
using SpaceHacathon.BattleScene.GUI.Screens;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.States {

    public class ShipGUIState : IState<GUIStates, GUIEvents> {
        private readonly IEventsReceiver<GameManagerEvents> _gameManager;
        private readonly ShipGUI _shipGUI;

        public ShipGUIState(IEventsReceiver<GameManagerEvents> gameManager, ShipGUI shipGUI) {
            _gameManager = gameManager;
            _shipGUI = shipGUI;
        }

        public override void Initiate() {
            _shipGUI.Hide();
        }

        public override void Enter() {
            _shipGUI.Show();
        }

        public override void Exit() {
            _shipGUI.Hide();
        }

        public override void Pause() {
            _shipGUI.Hide();
        }

        public override void Resume() {
            _shipGUI.Show();
        }

        public override StateRunResult<GUIStates> HandleEvents(GUIEvents nextEvent) {
            switch (nextEvent) {
                case GUIEvents.GamePaused:
                    return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.Push, ReturnState = GUIStates.PauseMenu};
            }
            return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override StateRunResult<GUIStates> Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _gameManager.PushEvent(GameManagerEvents.PausePressed);
            }
            return new StateRunResult<GUIStates>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public override GUIStates GetType {
            get {
                return GUIStates.ShipGUI;
            }
        }
        
    }

}