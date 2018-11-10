using SpaceHacathon.BattleScene.Game.Time;
using Zenject;
using Random = System.Random;

namespace SpaceHacathon.BattleScene.Installers {

    public class BattleSceneInstaller : MonoInstaller {

        public override void InstallBindings() {
            GameLoopInstaller.Install(Container);
            
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

    }

}