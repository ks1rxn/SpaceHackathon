using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using SpaceHacathon.Helpers;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours {

    public class RollHullBehaviour : IBehaviour {
        private readonly HullVisualizationComponent _hullVisualizationComponent;
        private readonly PhysicsComponent _physicsComponent;
        private readonly AccelerationComponent _accelerationComponent;
        private readonly TransformComponent _transformComponent;

        public RollHullBehaviour(HullVisualizationComponent hullVisualizationComponent, 
            PhysicsComponent physicsComponent, AccelerationComponent accelerationComponent, TransformComponent transformComponent) {
            
            _hullVisualizationComponent = hullVisualizationComponent;
            _physicsComponent = physicsComponent;
            _accelerationComponent = accelerationComponent;
            _transformComponent = transformComponent;
        }
        
        public void Run() {
            Vector3 needRotation = Vector3.zero;

            //roll
            float rollHullValue = _physicsComponent.AngularVelocity.y * (int)_accelerationComponent.ThrottleState * _hullVisualizationComponent.HullRollCoefficient;
            float maxRollValue = _hullVisualizationComponent.MaxHullRoll;
            needRotation.x = Mathf.Clamp(rollHullValue, -maxRollValue, maxRollValue);

            //pitch
            Vector3 pitchVector = _transformComponent.LookVector * _physicsComponent.Mass * _accelerationComponent.EnginePower;
            needRotation.z = -pitchVector.magnitude * (int) _accelerationComponent.ThrottleState * _hullVisualizationComponent.AccelerationPitchCoefficient;
            
            Vector3 rotationCorrection = _hullVisualizationComponent.PidController.Update(needRotation - MathHelper.AngleFrom360To180(_hullVisualizationComponent.Hull.localEulerAngles), Time.fixedDeltaTime);
            _hullVisualizationComponent.RotationSpeed += rotationCorrection * Time.fixedDeltaTime;
            _hullVisualizationComponent.Hull.Rotate(_hullVisualizationComponent.RotationSpeed * Time.fixedDeltaTime);
        }
        
    }

}