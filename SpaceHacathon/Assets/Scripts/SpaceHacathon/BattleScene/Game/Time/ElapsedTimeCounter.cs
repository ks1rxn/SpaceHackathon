using Zenject;

namespace SpaceHacathon.BattleScene.Game.Time {

    public class ElapsedTimeCounter : IInitializable, ITickable {
        public float ElapsedTime { get; private set; }

        public void Initialize() {
            ElapsedTime = 0;
        }

        public void Tick() {
            ElapsedTime += UnityEngine.Time.deltaTime;
        }
    }

}