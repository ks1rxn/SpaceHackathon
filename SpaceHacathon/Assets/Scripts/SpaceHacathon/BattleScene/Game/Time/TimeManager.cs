using System;
using Zenject;

namespace SpaceHacathon.BattleScene.Game.Time {

	[Obsolete]
	public class TimeManager : IInitializable, ITickable {
		private TimeScaleMode _timeScaleMode;
		private bool _onPause;

		public float GameTime { get; set; }

		public void SetTimeScaleMode(TimeScaleMode mode) {
			_timeScaleMode = mode;
		}

		public void Pause() {
			UnityEngine.Time.timeScale = 0;
			UnityEngine.Time.fixedDeltaTime = 0.02F * UnityEngine.Time.timeScale;
			_onPause = true;
		}

		public void Unpause() {
			_onPause = false;
			switch (_timeScaleMode) {
				case TimeScaleMode.Normal:
					UnityEngine.Time.timeScale = 1.0f;
					break;
				case TimeScaleMode.SuperSlow:
					UnityEngine.Time.timeScale = 0.1f;
					break;
			}
			UnityEngine.Time.fixedDeltaTime = 0.02F * UnityEngine.Time.timeScale;
		}

		private void UpdateTimeSpeed() {
			if (_onPause) {
				return;
			}
			switch (_timeScaleMode) {
				case TimeScaleMode.Normal:
					if (UnityEngine.Time.timeScale < 1) {
						UnityEngine.Time.timeScale += UnityEngine.Time.deltaTime * 8;
					}
					break;
				case TimeScaleMode.SuperSlow:
					if (UnityEngine.Time.timeScale > 0.1f) {
						UnityEngine.Time.timeScale -= UnityEngine.Time.deltaTime * 4;
					} else if (UnityEngine.Time.timeScale < 0.08) {
						UnityEngine.Time.timeScale += UnityEngine.Time.deltaTime * 4;
					}
					break;
			}
			UnityEngine.Time.fixedDeltaTime = 0.02F * UnityEngine.Time.timeScale;
		}

		public void Initialize() {
			GameTime = 0;

			UnityEngine.Time.timeScale = 1.0f;
			UnityEngine.Time.fixedDeltaTime = 0.02f * UnityEngine.Time.timeScale;
			_timeScaleMode = TimeScaleMode.Normal;
		}

		public void Tick() {
			UpdateTimeSpeed();

			GameTime += UnityEngine.Time.deltaTime;
			//todo: use this time to display in gui
		}
	}

}