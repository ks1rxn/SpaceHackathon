using System;
using System.Collections.Generic;

namespace SpaceHacathon.Helpers.FSM {

    public class StatesFactory<StatesEnum> {
        private readonly Dictionary<StatesEnum, IState<StatesEnum>> _states;

        public StatesFactory(List<IState<StatesEnum>> states) {
            _states = new Dictionary<StatesEnum, IState<StatesEnum>>(states.Count);
            foreach (IState<StatesEnum> state in states) {
                _states.Add(state.GetType, state);
            }
        }
        
        public IState<StatesEnum> GetState(StatesEnum state) {
            if (_states.ContainsKey(state)) {
                return _states[state];
            }
            
            throw new NotImplementedException($"Can\'t create state: {state}");
        }
        
    }

}