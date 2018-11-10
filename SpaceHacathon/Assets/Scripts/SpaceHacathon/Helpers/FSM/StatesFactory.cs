using System;
using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StatesFactory<StatesEnum, EventsEnum> {
        private readonly Dictionary<StatesEnum, IState<StatesEnum, EventsEnum>> _states;

        public StatesFactory(List<IState<StatesEnum, EventsEnum>> states) {
            _states = new Dictionary<StatesEnum, IState<StatesEnum, EventsEnum>>(states.Count);
            foreach (IState<StatesEnum, EventsEnum> state in states) {
                _states.Add(state.GetType, state);
            }
        }

        public IEnumerable<IState<StatesEnum, EventsEnum>> GetAllStates() {
            return _states.Values;
        }
        
        public IState<StatesEnum, EventsEnum> GetState(StatesEnum state) {
            if (_states.ContainsKey(state)) {
                return _states[state];
            }
            
            throw new NotImplementedException($"Can\'t create state: {state}");
        }
        
    }

}