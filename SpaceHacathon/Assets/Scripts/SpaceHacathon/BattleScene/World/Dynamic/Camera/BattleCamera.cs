using System.Collections;
using SpaceHacathon.Helpers.PidControllers;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.Camera {

	public class BattleCamera : MonoBehaviour {
		private ICameraTarget _objectToFollow;
		[SerializeField]
		private Animator _animator;
		
		private VectorPid _speedController;
		private Vector3 _speed;

		[Inject]
		private void Construct(ICameraTarget objectToFollow, VectorPid pid) {
			_objectToFollow = objectToFollow;
			_speedController = pid;
			_speedController.Initiate(new Vector3(50000f, 20000, 45000));
		}
		
		private void FixedUpdate() {
			Vector3 neededPosition = _objectToFollow.Position + _objectToFollow.Speed * 0.2f;
			neededPosition.y = 7.5f;
			neededPosition.z -= 9;

			Vector3 speedCorrection = _speedController.Update(neededPosition - transform.position, Time.fixedDeltaTime);
			_speed += 0.0003f * speedCorrection * Time.fixedDeltaTime;
			transform.Translate(0.4f * _speed * Time.fixedDeltaTime);
			Vector3 pos = transform.position;
			pos.y = 7.5f;
			transform.position = pos;

			transform.eulerAngles = new Vector3(50, 0, 0);
		}

		public void Shake() {
			_animator.SetBool("shaking", true);
			StartCoroutine(DelayedShakeDropParam());
		}

		private IEnumerator DelayedShakeDropParam() {
			yield return new WaitForEndOfFrame();
			_animator.SetBool("shaking", false);
		}

	}

}
