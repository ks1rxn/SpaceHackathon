using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours {

    public class AccelerationBehaviour : IBehaviour {
        private readonly PhysicsComponent _physicsComponent;
        private readonly TransformComponent _transformComponent;
        private readonly AccelerationComponent _accelerationComponent;

        public AccelerationBehaviour(PhysicsComponent physicsComponent, TransformComponent transformComponent, AccelerationComponent accelerationComponent) {
            _physicsComponent = physicsComponent;
            _transformComponent = transformComponent;
            _accelerationComponent = accelerationComponent;
        }

        public void Run() {
//            float engineForce = (int) _power * m_shipParams.EnginePower * m_settings.AccelerationCoefficient;
            const float accelerationCoefficient = 1.0f;
            float engineForce = (int) _accelerationComponent.ThrottleState * _accelerationComponent.EnginePower * accelerationCoefficient;
//            _physicsComponent.AddForce(engineForce * _transformComponent.LookVector * _physicsComponent.Mass * m_effects.Slowing * Time.fixedDeltaTime);
            const float slowing = 1.0f;
            //todo: get rid of deltaTime
            _physicsComponent.AddForce(engineForce * _transformComponent.LookVector * _physicsComponent.Mass * slowing * Time.fixedDeltaTime);
        }
        
    }

}