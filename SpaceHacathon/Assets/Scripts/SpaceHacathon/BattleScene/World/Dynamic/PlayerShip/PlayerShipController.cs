using System.Collections.Generic;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip {

    public class PlayerShipController : MonoBehaviour {
        private List<IBehaviour> _behaviours; 
        
        [Inject]
        public void Construct(RotationBehaviour rotationBehaviour, AccelerationBehaviour accelerationBehaviour, 
            ConstraintsCheckingBehaviour constraintsCheckingBehaviour, EnginesVisualizationBehaviour enginesVisualizationBehaviour,
            RollHullBehaviour rollHullBehaviour) {
            
            _behaviours = new List<IBehaviour>();
            
            _behaviours.Add(rotationBehaviour);
            _behaviours.Add(accelerationBehaviour);
            _behaviours.Add(constraintsCheckingBehaviour);
            _behaviours.Add(rollHullBehaviour);
            _behaviours.Add(enginesVisualizationBehaviour);
        }
        
        private void FixedUpdate() {
            foreach (IBehaviour behaviour in _behaviours) {
                behaviour.Run();
            }
        }
        
    }

}