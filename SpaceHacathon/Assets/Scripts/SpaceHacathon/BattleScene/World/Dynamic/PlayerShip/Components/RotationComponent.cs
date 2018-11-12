using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class RotationComponent : MonoBehaviour {
        public float DesiredAngle { get; set; }
        public float RemainedAngleToDesired { get; set; }
    }

}