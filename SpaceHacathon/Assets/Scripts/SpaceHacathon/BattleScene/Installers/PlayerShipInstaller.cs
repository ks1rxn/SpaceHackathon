using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Behaviours;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.Components;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip.States;
using SpaceHacathon.Helpers.FSM;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class PlayerShipInstaller : MonoInstaller {
        [SerializeField]
        private GameObject _componentsHolder;
        
        public override void InstallBindings() {
            Container.Bind<PlayerShipController>().FromComponentOn(gameObject).AsSingle();
            
            InstallComponents();
            InstallFlyingNormalState();
            InstallStateMachine();
        }

        private void InstallComponents() {
            Container.Bind<TransformComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<PhysicsComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<RotationComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<AccelerationComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<EnginesVisualizationComponent>().FromComponentOn(_componentsHolder).AsSingle();
            Container.Bind<HullVisualizationComponent>().FromComponentOn(_componentsHolder).AsSingle();
        }

        private void InstallFlyingNormalState() {
            Container.Bind<IBehaviour>().To<RotationBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
            Container.Bind<IBehaviour>().To<AccelerationBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
            Container.Bind<IBehaviour>().To<ConstraintsCheckingBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
            Container.Bind<IBehaviour>().To<EnginesVisualizationBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
            Container.Bind<IBehaviour>().To<RollHullBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
        }

        private void InstallStateMachine() {
            Container.Bind<StateMachine<PlayerShipStates, PlayerShipEvents>>().WhenInjectedInto<PlayerShipController>().NonLazy();
            Container.Bind<StatesFactory<PlayerShipStates, PlayerShipEvents>>().WhenInjectedInto<StateMachine<PlayerShipStates, PlayerShipEvents>>();
            
            Container.Bind<IState<PlayerShipStates, PlayerShipEvents>>().To<FlyingNormalState>().WhenInjectedInto<StatesFactory<PlayerShipStates, PlayerShipEvents>>();
            Container.Bind<IState<PlayerShipStates, PlayerShipEvents>>().To<DeadState>().WhenInjectedInto<StatesFactory<PlayerShipStates, PlayerShipEvents>>();
        }
        
    }

}