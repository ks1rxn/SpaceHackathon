using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;

namespace SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours {

    public class ConstraintsCheckingBehaviour {
        private readonly PhysicsComponent _physicsComponent;

        public ConstraintsCheckingBehaviour(PhysicsComponent physicsComponent) {
            _physicsComponent = physicsComponent;
        }
        
        public void Run() {
//            _physicsComponent.ClampVelocity(5 * m_effects.Slowing);
            const float slowing = 1.0f;
            _physicsComponent.ClampVelocity(5 * slowing);
            //todo: remove magic constant 5(max speed - ship param)
        }
        
    }

}