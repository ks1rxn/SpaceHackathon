namespace SpaceHacathon.BattleScene.Game.Loop {

    public struct GameLoopEvent {
        public Type EventType { get; set; }
        
        public enum Type {
            PausePressed,
            ResumePressed
        }
        
    }
    
}