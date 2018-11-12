using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours {

    public class AccelerationBehaviour {
        private readonly PhysicsComponent _physicsComponent;
        private readonly TransformComponent _transformComponent;
        private readonly ShipControlsComponent _shipControls;

        public AccelerationBehaviour(PhysicsComponent physicsComponent, TransformComponent transformComponent, ShipControlsComponent shipControls) {
            _physicsComponent = physicsComponent;
            _transformComponent = transformComponent;
            _shipControls = shipControls;
        }

        public void Run() {
//            float engineForce = (int) _power * m_shipParams.EnginePower * m_settings.AccelerationCoefficient;
            const float enginePower = 900;
            const float accelerationCoefficient = 1.0f;
            float engineForce = (int) _shipControls.ThrottleState * enginePower * accelerationCoefficient;
//            _physicsComponent.AddForce(engineForce * _transformComponent.LookVector * _physicsComponent.Mass * m_effects.Slowing * Time.fixedDeltaTime);
            const float slowing = 1.0f;
            _physicsComponent.AddForce(engineForce * _transformComponent.LookVector * _physicsComponent.Mass * slowing * Time.fixedDeltaTime);
        }
        
    }

}