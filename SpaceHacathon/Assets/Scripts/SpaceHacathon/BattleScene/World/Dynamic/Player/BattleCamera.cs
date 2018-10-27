using System.Collections;
using SpaceHacathon.Helpers.PidControllers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Dynamic.Player {

	public class BattleCamera : MonoBehaviour {
		private PlayerShipFacade _playerShipFacade;
		[SerializeField]
		private Animator _animator;
		
		private readonly VectorPid _speedController = new VectorPid(50000f, 20000, 45000);
		private Vector3 _speed;

		[Inject]
		private void Construct(PlayerShipFacade playerShipFacade) {
			_playerShipFacade = playerShipFacade;
		}
		
		private void LateUpdate() {
			Vector3 neededPosition = _playerShipFacade.Position + _playerShipFacade.SpeedVector * 0.2f;
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
