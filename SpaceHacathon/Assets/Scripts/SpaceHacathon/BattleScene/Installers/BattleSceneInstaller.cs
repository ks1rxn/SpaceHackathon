using System;
using SpaceHacathon.BattleScene.Game;
using SpaceHacathon.BattleScene.Game.Time;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class BattleSceneInstaller : MonoInstaller {

        public override void InstallBindings() {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            
            InstallTime();
            InstallMisc();
        }

        private void InstallTime() {
            Container.Bind<TimeSpeedController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ElapsedTimeCounter>().AsSingle();
        }
        
        private void InstallMisc() {
            Container.Bind<Random>().AsSingle();
        }
        
    }

}