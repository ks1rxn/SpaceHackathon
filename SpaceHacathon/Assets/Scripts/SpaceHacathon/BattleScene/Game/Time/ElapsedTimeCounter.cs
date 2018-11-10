namespace SpaceHacathon.BattleScene.Game.Time {

    public class ElapsedTimeCounter {
        public float ElapsedTime { get; private set; }

        public void Initiate() {
            ElapsedTime = 0;
        }

        public void AddTime(float time) {
            ElapsedTime += time;
        }
        
    }

}