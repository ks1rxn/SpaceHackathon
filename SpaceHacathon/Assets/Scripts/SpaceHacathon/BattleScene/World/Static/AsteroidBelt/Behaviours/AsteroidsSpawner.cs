using Zenject;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Behaviours {

    public class AsteroidsSpawner {
        private SimpleAsteroid.Factory _asteroidsFactory;
        
        [Inject]
        public void Construct(SimpleAsteroid.Factory asteroidsFactory) {
            _asteroidsFactory = asteroidsFactory;
        }
        
//        private SimpleAsteroid CreateNewAsteroid() {
//            int index = _random.Next(3) + 1;
//            SimpleAsteroid asteroid = _asteroidsFactory.Create($"Prefabs/Asteroids/Asteroid{index}");
//            asteroid.transform.parent = transform;
//            _asteroids.Add(asteroid);
//            return asteroid;
//        }
        
    }

}