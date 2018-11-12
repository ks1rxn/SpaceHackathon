using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class ShipControlsComponent : MonoBehaviour {
        public float DesiredAngle { get; set; }
        public ThrottleState ThrottleState { get; set; }
    }

}