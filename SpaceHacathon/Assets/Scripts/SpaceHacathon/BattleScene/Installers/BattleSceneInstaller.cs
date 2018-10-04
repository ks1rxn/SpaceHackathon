using System;
using SpaceHacathon.BattleScene.World.Static.Asteroids;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class BattleSceneInstaller : MonoInstaller {

        public override void InstallBindings() {
            InstallMisc();
        }

        private void InstallMisc() {
            Container.Bind<Random>().AsSingle();
        }
        
    }

}