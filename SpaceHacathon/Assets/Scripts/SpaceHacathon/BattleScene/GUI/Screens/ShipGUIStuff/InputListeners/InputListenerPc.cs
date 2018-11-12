using SpaceHacathon.Helpers;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners {

    public class InputListenerPc : IInputListener {
        private Vector3 _rotationJoystickPosition;
        
        public void Initiate(Vector3 rotationJoystickPosition) {
            _rotationJoystickPosition = rotationJoystickPosition;
        }

        public UserInput Listen() {
            UserInput input = new UserInput{IsAngleSet = false};

            if (IsAngleSet()) {
                input.IsAngleSet = true;
                input.NewAngle = GetNewAngle();
            }
            input.ThrottleState = GetThrottleState();
            input.PausePressed = Input.GetAxis("Cancel") > 0;
            input.ChargePressed = Input.GetKeyDown(KeyCode.Space);
            
            return input;
        }

        private bool IsAngleSet() {
            if (Input.GetAxis("Fire1") > 0) {
                float d = Vector3.Distance(Input.mousePosition, _rotationJoystickPosition);
                if (d < Screen.width / 3f) {
                    return true;
                }
            }
            return false;
        }

        private float GetNewAngle() {
            float newAngle = MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), Input.mousePosition - _rotationJoystickPosition);
            return newAngle;
        }
        
        private static ThrottleState GetThrottleState() {
            if (Input.GetAxis("Vertical") > 0) {
                return ThrottleState.Forward;
            }

            if (Input.GetAxis("Vertical") < 0) {
                return ThrottleState.Backward;
            }

            return ThrottleState.Off;
        }
        
    }

}