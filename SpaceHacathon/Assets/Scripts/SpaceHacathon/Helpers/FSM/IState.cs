using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Loop;

namespace SpaceHacathon.Helpers.FSM {

    public abstract class IState<StatesEnum> {

        public virtual void Enter() {}

        public virtual void Exit() { }

        public virtual StateRunResult<StatesEnum> HandleEvents(Queue<GameLoopEvent> events) {
            return new StateRunResult<StatesEnum>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public virtual StateRunResult<StatesEnum> Update() {
            return new StateRunResult<StatesEnum>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public virtual void Pause() { }

        public virtual void Resume() { }
        
        public abstract StatesEnum GetType { get; }
        
    }

}