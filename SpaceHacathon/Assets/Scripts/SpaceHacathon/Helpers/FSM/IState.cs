namespace SpaceHacathon.Helpers.FSM {

    public abstract class IState<TStatesEnum, TEventsEnum> {

        public virtual void Initiate() {}
        
        public virtual void Enter() {}

        public virtual void Exit() {}

        public virtual StateRunResult<TStatesEnum> HandleEvents(TEventsEnum nextEvent) {
            return new StateRunResult<TStatesEnum>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public virtual StateRunResult<TStatesEnum> Update() {
            return new StateRunResult<TStatesEnum>{StateRunReturnAction = StateRunReturnAction.None};
        }

        public virtual void Pause() {}

        public virtual void Resume() {}
        
        public abstract TStatesEnum GetType { get; }
        
    }

}