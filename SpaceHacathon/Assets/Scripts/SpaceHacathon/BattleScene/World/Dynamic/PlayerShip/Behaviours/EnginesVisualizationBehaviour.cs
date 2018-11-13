using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours {

    public class EnginesVisualizationBehaviour : IBehaviour {
        private readonly EnginesVisualizationComponent _enginesVisualizationComponent;
        private readonly AccelerationComponent _accelerationComponent;
        private readonly PhysicsComponent _physicsComponent;

        public EnginesVisualizationBehaviour(EnginesVisualizationComponent enginesVisualizationComponent, 
            AccelerationComponent accelerationComponent, PhysicsComponent physicsComponent) {
            
            _enginesVisualizationComponent = enginesVisualizationComponent;
            _accelerationComponent = accelerationComponent;
            _physicsComponent = physicsComponent;
        }
        
        public void Run() {
            UpdateRotationEngines();
            UpdateRunEngines();
        }

        private void UpdateRotationEngines() {
            if (Mathf.Abs(_physicsComponent.AngularVelocity.y) <= 0.5f) {
                _enginesVisualizationComponent.SetEngineState(3, false);
                _enginesVisualizationComponent.SetEngineState(6, false);
                _enginesVisualizationComponent.SetEngineState(4, false);
                _enginesVisualizationComponent.SetEngineState(5, false);
                return;
            }

            float powerCoef = 1;
            if (_accelerationComponent.ThrottleState == ThrottleState.Backward) {
                powerCoef = -1;
            }
            if (_physicsComponent.AngularVelocity.y * powerCoef < 0) {
                _enginesVisualizationComponent.SetEngineState(3, true);
                _enginesVisualizationComponent.SetEngineState(6, true);
                _enginesVisualizationComponent.SetEngineState(4, false);
                _enginesVisualizationComponent.SetEngineState(5, false);
            } else {
                _enginesVisualizationComponent.SetEngineState(3, false);
                _enginesVisualizationComponent.SetEngineState(6, false);
                _enginesVisualizationComponent.SetEngineState(4, true);
                _enginesVisualizationComponent.SetEngineState(5, true);
            }
        }
        
        private void UpdateRunEngines() {
            switch (_accelerationComponent.ThrottleState) {
                case ThrottleState.Forward:
                    _enginesVisualizationComponent.SetEngineState(0, true);
                    _enginesVisualizationComponent.SetEngineState(1, true);
                    _enginesVisualizationComponent.SetEngineState(2, true);
                    _enginesVisualizationComponent.SetEngineState(7, false);
                    _enginesVisualizationComponent.SetEngineState(8, false);
                    break;
                case ThrottleState.Backward:
                    _enginesVisualizationComponent.SetEngineState(0, false);
                    _enginesVisualizationComponent.SetEngineState(1, false);
                    _enginesVisualizationComponent.SetEngineState(2, false);
                    _enginesVisualizationComponent.SetEngineState(7, true);
                    _enginesVisualizationComponent.SetEngineState(8, true);
                    break;
                default:
                    _enginesVisualizationComponent.SetEngineState(0, false);
                    _enginesVisualizationComponent.SetEngineState(1, false);
                    _enginesVisualizationComponent.SetEngineState(2, false);
                    _enginesVisualizationComponent.SetEngineState(7, false);
                    _enginesVisualizationComponent.SetEngineState(8, false);
                    break;
            }
        }
        
    }

}