using Zenject;
using Random = System.Random;

namespace SpaceHacathon.BattleScene.Installers {

    public class BattleSceneInstaller : MonoInstaller {

        public override void InstallBindings() {
            GameLoopInstaller.Install(Container);

            InstallMisc();
            InstallSignals();
        }

        private void InstallSignals() {
            SignalBusInstaller.Install(Container);
        }
        
        private void InstallMisc() {
            Container.Bind<Random>().AsSingle();
        }

    }

}