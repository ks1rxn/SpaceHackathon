using JetBrains.Annotations;
using UnityEngine;

namespace SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners {

    public interface IInputListener {

        void Initiate(Vector3 rotationJoystickPosition);
        
        [Pure]
        UserInput Listen();

    }

}