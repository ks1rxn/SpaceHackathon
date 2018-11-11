using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.Player {

    public class PlayerShipFacade : MonoBehaviour {

        [Inject]
        private void Construct() {
            
        }

        public Vector3 Position => transform.position;

        public Vector3 SpeedVector { get; private set; }

    }

}