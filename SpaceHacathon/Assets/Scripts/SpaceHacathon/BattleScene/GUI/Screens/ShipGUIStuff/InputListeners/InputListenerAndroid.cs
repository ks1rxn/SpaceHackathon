using SpaceHacathon.Helpers;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners {

    public class InputListenerAndroid : IInputListener {
        private Vector3 _rotationJoystickPosition;
        
        public void Initiate(Vector3 rotationJoystickPosition) {
            _rotationJoystickPosition = rotationJoystickPosition;
        }
        
        public UserInput Listen() {
            UserInput input = new UserInput{IsAngleSet = false, ThrottleState = ThrottleState.Off, ChargePressed = false};
            
            input.PausePressed = UnityEngine.Input.GetAxis("Cancel") > 0;
            
            foreach (Touch touch in UnityEngine.Input.touches) {
                WhereTouchIs whereTouchIs = GetTouchPosition(touch);
                switch (whereTouchIs) {
                    case WhereTouchIs.RotationZone:
                        input.IsAngleSet = true;
                        input.NewAngle = GetNewAngle(touch.position);
                        break;
                    case WhereTouchIs.ThrottleZone:
                        input.ThrottleState = GetThrottleState(touch.position);
                        break;
                    case WhereTouchIs.ChargeZone:
                        input.ChargePressed = true;
                        break;
                }
            }

            return input;
        }

        private WhereTouchIs GetTouchPosition(Touch touch) {
            float distanceToAngle = Vector3.Distance(touch.position, _rotationJoystickPosition);
            if (distanceToAngle < Screen.width / 3f) {
                return WhereTouchIs.RotationZone;
            }

            if (touch.position.x < Screen.width / 4f && touch.position.y < Screen.height / 3f * 2f) {
                return WhereTouchIs.ThrottleZone;
            }

            if (touch.position.x > Screen.width / 4f && touch.position.x < Screen.width / 2f && touch.position.y < Screen.height / 4f) {
                return WhereTouchIs.ChargeZone;
            }

            return WhereTouchIs.None;
        }
        
        private float GetNewAngle(Vector3 touchPosition) {
            Vector3 position = new Vector3(touchPosition.x, touchPosition.y, 0);
            return MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), position - _rotationJoystickPosition);
        }
        
        private static ThrottleState GetThrottleState(Vector3 touchPosition) {
            int power = (int)Mathf.Sign(touchPosition.y - Screen.height / 4f);
            return power > 0 ? ThrottleState.Forward : ThrottleState.Backward;
        }
        
        private enum WhereTouchIs {
            None,
            RotationZone,
            ThrottleZone,
            ChargeZone
        }
        
    }

}