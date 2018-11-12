using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip {

    public class PlayerShipController : MonoBehaviour {
        private RotationBehaviour _rotationBehaviour;
        private AccelerationBehaviour _accelerationBehaviour;
        private ConstraintsCheckingBehaviour _constraintsCheckingBehaviour;
        
        [Inject]
        public void Construct(RotationBehaviour rotationBehaviour, AccelerationBehaviour accelerationBehaviour, ConstraintsCheckingBehaviour constraintsCheckingBehaviour) {
            _rotationBehaviour = rotationBehaviour;
            _accelerationBehaviour = accelerationBehaviour;
            _constraintsCheckingBehaviour = constraintsCheckingBehaviour;
        }
        
        private void FixedUpdate() {
            _rotationBehaviour.Run();
            _accelerationBehaviour.Run();
            _constraintsCheckingBehaviour.Run();
        }
        
    }

}