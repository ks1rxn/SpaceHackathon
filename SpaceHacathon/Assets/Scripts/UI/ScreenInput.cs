using UnityEngine;

public class ScreenInput : MonoBehaviour {
	private bool m_chargePressed;

	protected void Update() {
#if UNITY_EDITOR
		ListenPc();
#elif UNITY_ANDROID
		ListenAndroid();
#endif
	}

	private void ListenAndroid() {
		bool hasSpeedSetter = false;

		foreach (Touch touch in Input.touches) {
			float distanceToAngle = Vector3.Distance(touch.position, BattleContext.GUIController.Button.transform.position);
			if (distanceToAngle < Screen.width / 3f) {
				Vector3 position = new Vector3(touch.position.x, touch.position.y, 0);
				SetShipAngle(MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), position - BattleContext.GUIController.Button.transform.position));
			} else if ((touch.position.x < Screen.width / 4f) && (touch.position.y < Screen.height / 3f * 2f)) {
				float power = Mathf.Sign(touch.position.y - Screen.height / 4f);
				SetShipPower(power);
				hasSpeedSetter = true;
			} else if ((touch.position.x > Screen.width / 4f) && (touch.position.x < Screen.width / 2f) && (touch.position.y < Screen.height / 4f)) {
				Charge();
			}
		}
		if (!hasSpeedSetter) {
			SetShipPower(0);
		}
	}

	private void ListenPc() {
		// Ship angle //
		if (Input.GetMouseButton(0)) {
			float d = Vector3.Distance(Input.mousePosition, BattleContext.GUIController.Button.transform.position);
			if (d < Screen.width / 3f) {
				SetShipAngle(MathHelper.AngleBetweenVectorsZ(new Vector3(1, 0, 0), Input.mousePosition - BattleContext.GUIController.Button.transform.position));
			}
		}

		// Ship velocity //
		if (Input.GetKey(KeyCode.W)) {
			SetShipPower(1.0f);
		} else if (Input.GetKey(KeyCode.S)) {
			SetShipPower(-1.0f);
		} else {
			SetShipPower(0);
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			AddFuel();
		}

		// Ship charge //
		if (Input.GetKeyDown(KeyCode.Space)) {
			Charge();
		}
	}

	private void SetShipPower(float power) {
		BattleContext.PlayerShip.SetPower(power);
		BattleContext.GUIController.SetLeftJoysticValue(power);
	}

	private void SetShipAngle(float angle) {
		BattleContext.PlayerShip.SetAngle(angle);
		BattleContext.GUIController.SetRightJoystickAngle(angle);
	}

	private void Charge() {
		BattleContext.PlayerShip.Charge();
	}

	private void AddFuel() {
		BattleContext.PlayerShip.AddFuel();
	}

}
