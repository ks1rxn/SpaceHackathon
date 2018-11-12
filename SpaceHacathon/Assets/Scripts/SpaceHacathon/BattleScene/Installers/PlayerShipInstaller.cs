using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class PlayerShipInstaller : MonoInstaller {

        public override void InstallBindings() {
            Container.Bind<TransformComponent>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<PhysicsComponent>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<RotationComponent>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<AccelerationComponent>().FromComponentOn(gameObject).AsSingle();
            
            Container.Bind<PlayerShipController>().FromComponentOn(gameObject).AsSingle();

            Container.Bind<RotationBehaviour>().AsSingle();
            Container.Bind<AccelerationBehaviour>().AsSingle();
            Container.Bind<ConstraintsCheckingBehaviour>().AsSingle();
        }
        
    }

}