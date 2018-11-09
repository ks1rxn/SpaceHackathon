using System.Collections.Generic;

namespace SpaceHacathon.BattleScene.Game.Loop {

    public abstract class IState {

        public virtual void Enter() {}

        public virtual void Exit() { }

        public virtual GameLoopStateResult HandleEvents(Queue<GameLoopEvent> events) {
            return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.None};
        }

        public virtual GameLoopStateResult Update() {
            return new GameLoopStateResult{ReturnAction = GameLoopStateResult.Action.None};
        }

        public virtual void Pause() { }

        public virtual void Resume() { }

    }

}