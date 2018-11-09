using System;
using SpaceHacathon.BattleScene.Game.Loop.States;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public enum GameLoopStates {
        PlayNormal,
        InPause
    }
    
    public class StatesFactory {
        private readonly PlayNormalState.Factory _playNormalFactory;
        private readonly InPauseState.Factory _inPauseFactory;

        public StatesFactory(PlayNormalState.Factory playNormalFactory, InPauseState.Factory inPauseFactory) {
            _playNormalFactory = playNormalFactory;
            _inPauseFactory = inPauseFactory;
        }

        public IState GetState(GameLoopStates state) {
            switch (state) {
                case GameLoopStates.PlayNormal:
                    return _playNormalFactory.Create();
                case GameLoopStates.InPause:
                    return _inPauseFactory.Create();
            }

            throw new NotImplementedException("Can't create state: " + state);
        }
        
    }

}