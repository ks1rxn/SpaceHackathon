using SpaceHacathon.Helpers.PidControllers;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class HullVisualizationComponent : MonoBehaviour, IComponent {
        [SerializeField]
        private Transform _hull;
        [SerializeField]
        private Vector3 _pidValues = new Vector3(4.244681f, 0.1f, 1.25f);
        
        [Inject]
        public void Construct(VectorPid pidController) {
            PidController = pidController;
            PidController.Initiate(_pidValues);
        }

        public VectorPid PidController { get; private set; }
        public Vector3 RotationSpeed { get; set; }
        public Transform Hull => _hull;

        // Some constants:
        
        public float HullRollCoefficient => -15;
        public float AccelerationPitchCoefficient => 1 / 1500f;
        //prevent ship from showing his underwear
        public float MaxHullRoll => 23;

    }

}