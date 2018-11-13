using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using SpaceHacathon.Helpers;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours {

    public class RotationBehaviour : IBehaviour {
        private readonly TransformComponent _transformComponent;
        private readonly PhysicsComponent _physicsComponent;
        private readonly RotationComponent _rotationComponent;

        public RotationBehaviour(TransformComponent transformComponent, PhysicsComponent physicsComponent, RotationComponent rotationComponent) {
            _transformComponent = transformComponent;
            _physicsComponent = physicsComponent;
            _rotationComponent = rotationComponent;
        }

        public void Run() {
            float shortAngleToDesired = ShortAngleToDesired(_rotationComponent.DesiredAngle);
            float longAngleToDesired = LongAngleToDesired(shortAngleToDesired);
            float bestAngleForRotation = SelectBestAngleForRotation(shortAngleToDesired, longAngleToDesired, _physicsComponent.AngularVelocity);
            
            AddRotation(bestAngleForRotation);
            _rotationComponent.RemainedAngleToDesired = bestAngleForRotation;
        }
        
        private float ShortAngleToDesired(float desiredAngle)  {
            Vector3 vectorToTarget = new Vector3(Mathf.Cos(desiredAngle * Mathf.PI / 180), 0, Mathf.Sin(desiredAngle * Mathf.PI / 180));
            return MathHelper.AngleBetweenVectors(_transformComponent.LookVector, vectorToTarget);
        }

        private static float LongAngleToDesired(float shortAngleToDesired) {
            return -Mathf.Sign(shortAngleToDesired) * (360 - Mathf.Abs(shortAngleToDesired));
        }

        private static float SelectBestAngleForRotation(float shortAngle, float longAngle, Vector3 angularVelocity) {
            bool alreadyRotatingToLongSide = angularVelocity.y * longAngle > 0;
            bool rotationSpeedIsBigEnough = Mathf.Abs(angularVelocity.y) > 1;
            bool fasterToContinueRotation = Mathf.Abs(angularVelocity.y * 50) > Mathf.Abs(longAngle + shortAngle);
            
            if (alreadyRotatingToLongSide && rotationSpeedIsBigEnough && fasterToContinueRotation) {
                return longAngle;
            }

            return shortAngle;
        }

        private void AddRotation(float bestAngleForRotation) {
//            float angularForce = Mathf.Sign(actualAngle) * Mathf.Sqrt(Mathf.Abs(actualAngle)) * m_shipParams.RotationPower * m_settings.RotationCoefficient;
            const float rotationCoefficient = 1.0f;
            float angularForce = Mathf.Sign(bestAngleForRotation) * Mathf.Sqrt(Mathf.Abs(bestAngleForRotation)) * _rotationComponent.RotationPower * rotationCoefficient;
//            _physicComponent.AddTorque(new Vector3(0, angularForce * _physicComponent.Mass * m_effects.Slowing * Time.fixedDeltaTime, 0));
            const float slowing = 1.0f;
            _physicsComponent.AddTorque(new Vector3(0, angularForce * _physicsComponent.Mass * slowing * Time.fixedDeltaTime, 0));
        }
        
    }

}