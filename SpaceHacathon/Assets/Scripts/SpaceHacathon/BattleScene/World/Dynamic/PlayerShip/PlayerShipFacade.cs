using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff;
using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners;
using SpaceHacathon.BattleScene.World.Dynamic.Camera;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip {

    public class PlayerShipFacade : MonoBehaviour, IPlayerControllable, ICameraTarget {
        private TransformComponent _transformComponent;
        private PhysicsComponent _physicsComponent;
        private RotationComponent _rotationComponent;
        private AccelerationComponent _accelerationComponent;
        
        [Inject]
        private void Construct(TransformComponent transformComponent, PhysicsComponent physicsComponent, 
            RotationComponent rotationComponent, AccelerationComponent accelerationComponent) {
            
            _transformComponent = transformComponent;
            _physicsComponent = physicsComponent;
            _rotationComponent = rotationComponent;
            _accelerationComponent = accelerationComponent;
        }

        public void SetDesiredAngle(float desiredAngle) {
            _rotationComponent.DesiredAngle = desiredAngle;
        }

        public void SetThrottle(ThrottleState throttle) {
            _accelerationComponent.ThrottleState = throttle;
        }

        public void Charge() {
            
        }

        public PlayerShipOutput GetOutput() {
            PlayerShipOutput output = new PlayerShipOutput { DesiredAngle = _rotationComponent.DesiredAngle, RemainedAngleToDesired = _rotationComponent.RemainedAngleToDesired};
            return output;
        }

        public Vector3 Position => _transformComponent.Position;

        public Vector3 Speed => _physicsComponent.Velocity;

    }

}