namespace SpaceHacathon.BattleScene.Game.Time {

    public class TimeSpeedController {
        
        public void StopTime() {
            UnityEngine.Time.timeScale = 0;
            CorrectFixedDeltaTime();
        }

        public void ResumeTime() {
            UnityEngine.Time.timeScale = 1;
            CorrectFixedDeltaTime();
        }

        private static void CorrectFixedDeltaTime() {
            UnityEngine.Time.fixedDeltaTime = 0.02F * UnityEngine.Time.timeScale;
        }

    }

}