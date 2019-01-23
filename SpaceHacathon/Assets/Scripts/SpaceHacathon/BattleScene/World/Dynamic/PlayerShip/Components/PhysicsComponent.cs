using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class PhysicsComponent : MonoBehaviour, IComponent {
        [SerializeField]
        private Rigidbody _rigidbody;

        public void AddForce(Vector3 force) {
            _rigidbody.AddForce(force);
        }
        
        public void AddTorque(Vector3 torque) {
            _rigidbody.AddTorque(torque);
        }

        public void ClampVelocity(float maxVelocity) {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxVelocity);
        }
        
        public Vector3 Velocity => _rigidbody.velocity;

        public Vector3 AngularVelocity => _rigidbody.angularVelocity;

        public float Mass => _rigidbody.mass;

    }

}