namespace SpaceHacathon.Helpers.FSM {

    public struct StateRunResult<TStatesEnum> {
        public StateRunReturnAction StateRunReturnAction { get; set; }
        public TStatesEnum ReturnState { get; set; }    
    }
    
}