using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip {

    public class PlayerShipFacade : MonoBehaviour {
        private TransformComponent _transformComponent;
        private PhysicsComponent _physicsComponent;
        private ShipControlsComponent _shipControls;
        
        [Inject]
        private void Construct(TransformComponent transformComponent, PhysicsComponent physicsComponent, ShipControlsComponent shipControls) {
            _transformComponent = transformComponent;
            _physicsComponent = physicsComponent;
            _shipControls = shipControls;
        }

        public void SetAngle(float angle) {
            _shipControls.DesiredAngle = angle;
        }

        public void SetPower(ThrottleState throttleState) {
            _shipControls.ThrottleState = throttleState;
        }

        public void Charge() {
            
        }

        public Vector3 Position => _transformComponent.Position;

        public Vector3 SpeedVector => _physicsComponent.Velocity;

    }

}