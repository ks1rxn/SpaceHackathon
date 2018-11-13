using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class RotationComponent : MonoBehaviour {
        [SerializeField]
        private float _rotationPower = 70;

        public float RotationPower => _rotationPower;
        public float DesiredAngle { get; set; }
        public float RemainedAngleToDesired { get; set; }
    }

}