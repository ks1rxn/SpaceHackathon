using SpaceHacathon.BattleScene.Game.Loop;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.GUI {

    public class GUIManager : MonoBehaviour {
        private IGameLoop _gameLoop;
        private PlayerGUIController _playerGUI;
        private PauseMenu _pauseMenu;
        private DeathMenu _deathMenu;

        [Inject]
        private void Construct(IGameLoop gameLoop, PlayerGUIController playerGUI, PauseMenu pauseMenu, DeathMenu deathMenu) {
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
                _gameLoop.PushEvent(GameLoopEvents.PausePressed);
            }
        }
        
    }

}