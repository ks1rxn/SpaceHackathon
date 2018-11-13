using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class PlayerShipInstaller : MonoInstaller {
        [SerializeField]
        private GameObject _componentsHolder;
        
        public override void InstallBindings() {
            Container.Bind<PlayerShipController>().FromComponentOn(gameObject).AsSingle();
            InstallComponents();
            InstallBehaviours();
        }

        private void InstallComponents() {
            Container.Bind<TransformComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<PhysicsComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<RotationComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<AccelerationComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<EnginesVisualizationComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<HullVisualizationComponent>().FromComponentOn(_componentsHolder).AsSingle();
        }

        private void InstallBehaviours() {
            Container.Bind<RotationBehaviour>().AsSingle();
            Container.Bind<AccelerationBehaviour>().AsSingle();
            Container.Bind<ConstraintsCheckingBehaviour>().AsSingle();
            Container.Bind<EnginesVisualizationBehaviour>().AsSingle();
            Container.Bind<RollHullBehaviour>().AsSingle();
        }
        
    }

}