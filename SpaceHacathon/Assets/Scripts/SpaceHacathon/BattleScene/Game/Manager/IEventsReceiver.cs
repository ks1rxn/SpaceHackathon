namespace SpaceHacathon.BattleScene.Game.Manager {

    public interface IEventsReceiver<TEvents> {
        
        void PushEvent(TEvents newEvent);
        
    }

}