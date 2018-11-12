using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners;

namespace SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff {

    public interface IPlayerControllable {

        void SetDesiredAngle(float desiredAngle);

        void SetThrottle(ThrottleState throttle);
        
        void Charge();

        PlayerShipOutput GetOutput();

    }
    
}