using SpaceHacathon.BattleScene.World.Static.Asteroids;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class AsteroidsInstaller : MonoInstaller {
        
        public override void InstallBindings() {
            Container.BindFactory<string, AsteroidsBlock, AsteroidsBlock.Factory>().FromFactory<PrefabResourceFactory<AsteroidsBlock>>();
            Container.BindFactory<string, AsteroidsGroup, AsteroidsGroup.Factory>().FromFactory<PrefabResourceFactory<AsteroidsGroup>>();
            Container.BindFactory<string, Asteroid, Asteroid.Factory>().FromFactory<PrefabResourceFactory<Asteroid>>();
        }
        
    }

}