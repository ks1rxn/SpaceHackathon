using SpaceHacathon.BattleScene.World.Static.AsteroidBelt;
using SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Behaviours;
using SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Model;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class AsteroidsInstaller : MonoInstaller {
        
        public override void InstallBindings() {
            Container.Bind<IAsteroidBlocksSpawner>().To<AsteroidBlocksSpawner>().AsSingle();
            Container.Bind<IAsteroidBlocksFixer>().To<AsteroidBlocksFixer>().AsSingle();
            Container.BindFactory<string, AsteroidsBlock, AsteroidsBlock.Factory>().FromFactory<PrefabResourceFactory<AsteroidsBlock>>();
            
            Container.BindFactory<string, AsteroidsGroup, AsteroidsGroup.Factory>().FromFactory<PrefabResourceFactory<AsteroidsGroup>>();
            Container.BindFactory<string, Asteroid, Asteroid.Factory>().FromFactory<PrefabResourceFactory<Asteroid>>();
        }
        
    }

}