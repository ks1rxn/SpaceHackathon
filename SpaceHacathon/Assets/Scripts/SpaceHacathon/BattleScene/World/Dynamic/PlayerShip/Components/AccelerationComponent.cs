using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class AccelerationComponent : MonoBehaviour {
        [SerializeField]
        private float _enginePower = 900;

        public float EnginePower => _enginePower;
        public ThrottleState ThrottleState { get; set; }
    }

}