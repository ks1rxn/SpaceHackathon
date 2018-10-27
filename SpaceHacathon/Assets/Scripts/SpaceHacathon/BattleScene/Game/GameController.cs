using SpaceHacathon.BattleScene.Game.Time;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Game {

    public class GameController : IInitializable, ITickable {
        private readonly TimeSpeedController _timeSpeedController;
        
        private GameStates _gameState;

        public GameController(TimeSpeedController timeSpeedController) {
            _timeSpeedController = timeSpeedController;
        }
        
        public void Initialize() {
            ToPlaying();
        }

        public void Tick() {
            switch (_gameState) {
                case GameStates.Playing:
                    UpdatePlaying();
                    break;
                case GameStates.InPause:
                    UpdateInPause();
                    break;
            }
        }

        private void ToPlaying() {
            _gameState = GameStates.Playing;
            _timeSpeedController.ResumeTime();
        }
        
        private void UpdatePlaying() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToPause();    
            }
        }

        private void ToPause() {
            _gameState = GameStates.InPause;
            _timeSpeedController.StopTime();
        }
        
        private void UpdateInPause() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToPlaying();
            }
        }

    }

    public enum GameStates {
        Playing,
        InPause
    }

}