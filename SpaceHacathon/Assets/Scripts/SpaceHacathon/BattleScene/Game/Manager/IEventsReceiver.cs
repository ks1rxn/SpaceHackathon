namespace SpaceHacathon.BattleScene.Game.Manager {

    public interface IEventsReceiver<Events> {
        
        void PushEvent(Events newEvent);
        
    }

}