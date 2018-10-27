using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Behaviours {

    public class SimpleAsteroid : MonoBehaviour {
        private float _rotationSpeed;
        private Vector3 _rotationVector;

        public void Initiate(Vector3 position, Vector3 rotationVector, float size, float rotationSpeed) {
            transform.localPosition = position;
            
            _rotationVector = rotationVector;
            _rotationSpeed = rotationSpeed;
            transform.Rotate(_rotationVector, _rotationSpeed * 100);
            
            transform.localScale = new Vector3(size, size, size);
        }
        
        public void UpdateRotation() {
            transform.Rotate(_rotationVector, _rotationSpeed);
        }
        
        public class Factory : PlaceholderFactory<string, SimpleAsteroid> { }
    }

}