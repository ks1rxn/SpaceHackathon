namespace SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners {

    public struct UserInput {
        public bool IsAngleSet { get; set; }
        public float NewAngle { get; set; }
        public ThrottleState ThrottleState { get; set; }
        public bool PausePressed { get; set; }
        public bool ChargePressed { get; set; }
    }

}