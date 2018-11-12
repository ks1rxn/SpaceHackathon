using UnityEngine;
using UnityEngine.UI;

namespace SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.Elements {

    public class RotationJoystick : MonoBehaviour {
        [SerializeField]
        private Transform _desiredRotationIndicator;
        [SerializeField]
        private Image _rotationProgressIndicator;

        private float _desiredAngle;
        private float _remainedAngleToDesired;
        
        public void SetRotationParams(float desiredAngle, float remainedAngleToDesired) {
            _desiredAngle = desiredAngle;
            _remainedAngleToDesired = remainedAngleToDesired;
        }
        
        private void Update() {
            _rotationProgressIndicator.transform.eulerAngles = new Vector3(0, 0, _desiredAngle);
            _rotationProgressIndicator.fillClockwise = _remainedAngleToDesired < 0;
            _rotationProgressIndicator.fillAmount = Mathf.Abs(_remainedAngleToDesired) / 360f;
            
            _desiredRotationIndicator.transform.eulerAngles = new Vector3(0, 0, -60 + _desiredAngle);
        }
        
        public Vector3 Position => transform.position;
    }

}