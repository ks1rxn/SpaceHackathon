using System;
using SpaceHacathon.BattleScene.Game;
using SpaceHacathon.BattleScene.Game.Loop;
using SpaceHacathon.BattleScene.Game.Loop.States;
using SpaceHacathon.BattleScene.Game.Time;
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
            
            Container.Bind<StatesFactory>().AsSingle();
            Container.BindFactory<PlayNormalState, PlayNormalState.Factory>().WhenInjectedInto<StatesFactory>();
            Container.BindFactory<InPauseState, InPauseState.Factory>().WhenInjectedInto<StatesFactory>();
        }
        
    }

}