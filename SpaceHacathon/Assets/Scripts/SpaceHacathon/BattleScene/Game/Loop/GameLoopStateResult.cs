namespace SpaceHacathon.BattleScene.Game.Loop {

    public struct GameLoopStateResult {
        public Action ReturnAction { get; set; }
        public GameLoopStates ReturnState { get; set; }
        
        public enum Action {
            None,
            Change,
            Pop,
            Push
        }
        
    }

}