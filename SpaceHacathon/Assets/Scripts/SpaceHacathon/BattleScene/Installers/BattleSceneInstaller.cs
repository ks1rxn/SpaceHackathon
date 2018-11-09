using System;
using SpaceHacathon.BattleScene.Game.Loop;
using SpaceHacathon.BattleScene.Game.Loop.States;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class BattleSceneInstaller : MonoInstaller {

        public override void InstallBindings() {
            //todo: layergui, pausemenu - mv-s. guicontroller - gocontext. all outside gui communication - signals
            
            InstallGameLoop();
            
            InstallTime();
            InstallMisc();
            InstallSignals();
        }

        private void InstallSignals() {
            SignalBusInstaller.Install(Container);
        }
        
        private void InstallTime() {
            Container.Bind<TimeSpeedController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ElapsedTimeCounter>().AsSingle();
        }
        
        private void InstallMisc() {
            Container.Bind<Random>().AsSingle();
        }

        private void InstallGameLoop() {
            Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle();
            
            Container.Bind<StateMachine<GameLoopStates>>().WhenInjectedInto<GameLoop>();
            Container.Bind<StatesFactory<GameLoopStates>>().WhenInjectedInto<StateMachine<GameLoopStates>>();
            
            Container.Bind<IState<GameLoopStates>>().To<PlayNormalState>().WhenInjectedInto<StatesFactory<GameLoopStates>>();
            Container.Bind<IState<GameLoopStates>>().To<InPauseState>().WhenInjectedInto<StatesFactory<GameLoopStates>>();
        }
        
    }

}