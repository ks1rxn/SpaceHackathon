namespace SpaceHacathon.Helpers.FSM {

    public struct StateRunResult<StatesEnum> {
        public StateRunReturnAction StateRunReturnAction { get; set; }
        public StatesEnum ReturnState { get; set; }    
    }
    
}