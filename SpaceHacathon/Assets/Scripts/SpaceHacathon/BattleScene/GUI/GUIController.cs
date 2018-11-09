using SpaceHacathon.BattleScene.Game.Loop;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.GUI {

    public class GUIController : MonoBehaviour {
        private GameLoop _gameLoop;
        private PlayerGUIController _playerGUI;
        private PauseMenu _pauseMenu;
        private DeathMenu _deathMenu;

        [Inject]
        private void Construct(GameLoop gameLoop, PlayerGUIController playerGUI, PauseMenu pauseMenu, DeathMenu deathMenu) {
            _gameLoop = gameLoop;
            _playerGUI = playerGUI;
            _pauseMenu = pauseMenu;
            _deathMenu = deathMenu;
        }

        //todo: bad idea: inject PlayerShip into GUI layer. Use abstraction IControllable instead?
        //todo: use abstraction in GameController too??
        private void Start() {
            
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _gameLoop.PushEvent(new GameLoopEvent{EventType = GameLoopEvent.Type.PausePressed});
            }
        }
        
    }

}