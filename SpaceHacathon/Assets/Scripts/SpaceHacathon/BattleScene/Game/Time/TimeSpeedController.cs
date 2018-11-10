namespace SpaceHacathon.BattleScene.Game.Time {

    public class TimeSpeedController {
        
        public void StopTime() {
            UnityEngine.Time.timeScale = 0;
            AdjustFixedDeltaTime();
        }

        public void ResumeTime() {
            UnityEngine.Time.timeScale = 1;
            AdjustFixedDeltaTime();
        }

        private static void AdjustFixedDeltaTime() {
            UnityEngine.Time.fixedDeltaTime = 0.02F * UnityEngine.Time.timeScale;
        }

    }

}