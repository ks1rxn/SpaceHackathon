namespace SpaceHacathon.Helpers.FSM {

    public abstract class IState<StatesEnum, EventsEnum> {

        public virtual void Initiate() {}
        
        public virtual void Enter() {}

        public virtual void Exit() {}

        public virtual StateRunResult<StatesEnum> HandleEvents(EventsEnum nextEvent) {
            return new StateRunResult<StatesEnum>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public virtual StateRunResult<StatesEnum> Update() {
            return new StateRunResult<StatesEnum>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public virtual void Pause() {}

        public virtual void Resume() {}
        
        public abstract StatesEnum GetType { get; }
        
    }

}