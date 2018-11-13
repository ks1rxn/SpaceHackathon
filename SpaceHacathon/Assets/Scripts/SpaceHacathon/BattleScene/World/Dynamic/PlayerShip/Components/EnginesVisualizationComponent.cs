using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class EnginesVisualizationComponent : MonoBehaviour {
        [SerializeField]
        private ParticleSystem[] _engines;
        
        private bool[] _engineStates;
        private bool[] EngineStates => _engineStates ?? (_engineStates = new bool[_engines.Length]);

        public void SetEngineState(int engineIndex, bool newEngineState) {
            if (EngineStates[engineIndex] == newEngineState) {
                return;
            }
             
            EngineStates[engineIndex] = newEngineState;
            if (newEngineState) {
                _engines[engineIndex].Play();
            } else {
                _engines[engineIndex].Stop();
            }
        }
    }

}