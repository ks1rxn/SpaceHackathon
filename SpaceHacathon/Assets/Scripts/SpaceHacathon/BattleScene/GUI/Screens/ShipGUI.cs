using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff;
using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.Elements;
using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.GUI.Screens {

    public class ShipGUI : MonoBehaviour {
        private IInputListener _inputListener;
        private IPlayerControllable _playerShip;
        
        [SerializeField]
        private RotationJoystick _rotationJoystick;

        [Inject]
        public void Construct(IInputListener inputListener, IPlayerControllable playerShip) {
            _inputListener = inputListener;
            _playerShip = playerShip;
        }

        public void Initiate() {
            _inputListener.Initiate(_rotationJoystick.Position);
        }
        
        public void Show() {
            gameObject.SetActive(true);
        }

        private void Update() {
            UserInput input = _inputListener.Listen();
            SendPlayerShipInput(input);
            ReceivePlayerShipOutput();
        }

        private void SendPlayerShipInput(UserInput input) {
            if (input.IsAngleSet) {
                _playerShip.SetDesiredAngle(input.NewAngle);
            }
            _playerShip.SetThrottle(input.ThrottleState);
        }

        private void ReceivePlayerShipOutput() {
            PlayerShipOutput output = _playerShip.GetOutput();
            _rotationJoystick.SetRotationParams(output.DesiredAngle, output.RemainedAngleToDesired);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
        
    }

}