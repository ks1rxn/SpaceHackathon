﻿using SpaceHacathon.BattleScene.GUI.Screens.ShipGUIStuff.InputListeners;
using SpaceHacathon.Helpers;
using UnityEngine;

public class ScreenInput : MonoBehaviour {
	private bool m_chargePressed;

	public void ListerInput() {
#if UNITY_EDITOR
		ListenPc();
#elif UNITY_ANDROID
		ListenAndroid();
#endif
	}

	private void ListenAndroid() {
		if (Input.GetAxis("Cancel") > 0) {
			BattleContext.BattleManager.Director.OnPauseGame();
			return;
		}

		bool hasSpeedSetter = false;

		foreach (Touch touch in Input.touches) {
			float distanceToAngle = Vector3.Distance(touch.position, BattleContext.BattleManager.GUIManagerObsolete.PlayerGUIController.RotationJoystickCenter);
			if (distanceToAngle < Screen.width / 3f) {
				Vector3 position = new Vector3(touch.position.x, touch.position.y, 0);
				SetShipAngle(MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), position - BattleContext.BattleManager.GUIManagerObsolete.PlayerGUIController.RotationJoystickCenter));
			} else if ((touch.position.x < Screen.width / 4f) && (touch.position.y < Screen.height / 3f * 2f)) {
				int power = (int)Mathf.Sign(touch.position.y - Screen.height / 4f);
				SetShipPower(power > 0 ? ThrottleState.Forward : ThrottleState.Backward);
				hasSpeedSetter = true;
			} else if ((touch.position.x > Screen.width / 4f) && (touch.position.x < Screen.width / 2f) && (touch.position.y < Screen.height / 4f)) {
				Charge();
			}
		}
		if (!hasSpeedSetter) {
			SetShipPower(ThrottleState.Off);
		}
	}

	private void ListenPc() {
		if (Input.GetAxis("Cancel") > 0) {
			BattleContext.BattleManager.Director.OnPauseGame();
			return;
		}

		// Ship angle //
		if (Input.GetAxis("Fire1") > 0) {
			float d = Vector3.Distance(Input.mousePosition, BattleContext.BattleManager.GUIManagerObsolete.PlayerGUIController.RotationJoystickCenter);
			if (d < Screen.width / 3f) {
				SetShipAngle(MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), Input.mousePosition - BattleContext.BattleManager.GUIManagerObsolete.PlayerGUIController.RotationJoystickCenter));
			}
		}

		// Ship velocity //
		
		if (Input.GetAxis("Vertical") > 0) {
			SetShipPower(ThrottleState.Forward);
		} else if (Input.GetAxis("Vertical") < 0) {
			SetShipPower(ThrottleState.Backward);
		} else {
			SetShipPower(ThrottleState.Off);
		}

		// Ship charge //
		if (Input.GetKeyDown(KeyCode.Space)) {
			Charge();
		}
	}

	private void SetShipPower(ThrottleState power) {
		BattleContext.BattleManager.Director.PlayerShip.SetPower(power);
	}

	private void SetShipAngle(float angle) {
		BattleContext.BattleManager.Director.PlayerShip.SetAngle(angle);
	}

	private void Charge() {
		BattleContext.BattleManager.Director.PlayerShip.Charge();
	}

}
