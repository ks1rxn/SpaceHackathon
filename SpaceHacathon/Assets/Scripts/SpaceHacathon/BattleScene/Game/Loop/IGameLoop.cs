namespace SpaceHacathon.BattleScene.Game.Loop {

    public interface IGameLoop {
        
        void PushEvent(GameLoopEvents newEvent);
        
    }

}