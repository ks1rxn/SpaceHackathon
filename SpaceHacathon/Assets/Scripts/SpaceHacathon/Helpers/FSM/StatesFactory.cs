using System;
using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StatesFactory<TStatesEnum, TEventsEnum> {
        private readonly Dictionary<TStatesEnum, IState<TStatesEnum, TEventsEnum>> _states;

        public StatesFactory(List<IState<TStatesEnum, TEventsEnum>> states) {
            _states = new Dictionary<TStatesEnum, IState<TStatesEnum, TEventsEnum>>(states.Count);
            foreach (IState<TStatesEnum, TEventsEnum> state in states) {
                _states.Add(state.GetType, state);
            }
        }

        public IEnumerable<IState<TStatesEnum, TEventsEnum>> GetAllStates() {
            return _states.Values;
        }
        
        public IState<TStatesEnum, TEventsEnum> GetState(TStatesEnum state) {
            if (_states.ContainsKey(state)) {
                return _states[state];
            }
            
            throw new NotImplementedException($"Can\'t create state: {state}");
        }
        
    }

}