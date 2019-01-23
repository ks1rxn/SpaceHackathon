using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components {

    public class TransformComponent : MonoBehaviour, IComponent {
        [SerializeField]
        private Transform _targetTransform;
        
        public Vector3 Position => _targetTransform.position;

        public Vector3 LookVector => new Vector3(Mathf.Cos(-_targetTransform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-_targetTransform.rotation.eulerAngles.y * Mathf.PI / 180));

    }

}