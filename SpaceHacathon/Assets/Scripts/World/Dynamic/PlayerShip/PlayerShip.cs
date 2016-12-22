using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	private float m_neededAngle;
	private float m_power;

	private ShipState m_state;
	private EffectsOnShip m_effects;
	private ShipParams m_shipParams;

    [SerializeField]
    private PlayerShipHull m_hull;
	[SerializeField]
	private PlayerShipChargeSystem m_chargeSystem;
	[SerializeField]
	private ParticleSystem m_stunFx;
	[SerializeField]
	private ParticleSystem m_ps;

	private void Awake() {
		BattleContext.PlayerShip = this;

        m_hull.Initiate();
        m_chargeSystem.Initiate();

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		m_state = ShipState.OnMove;
		m_effects = new EffectsOnShip();
		m_shipParams = new ShipParams();
	}

	public void BlasterHit() {
		m_hull.Hit(0.1f);
		StopCoroutine("StunProcedure");
		StartCoroutine(StunProcedure());
	}

	public void RocketHit(Vector3 position) {
		//hack
		position.y = 0;
		m_rigidbody.AddExplosionForce(m_rigidbody.mass * 50, Position + (position - Position).normalized * 3, 5);
		m_hull.Hit(0.5f);
		StopCoroutine("BashProcedure");
		StartCoroutine(BashProcedure());
	}

	public void AddFuel() {
		m_chargeSystem.AddFuel();
	}

    public void Die() {
        if (m_state == ShipState.Dead) {
            return;
        }
        m_state = ShipState.Dead;
		BattleContext.ExplosionsController.PlayerShipExplosion(transform.position);
		BattleContext.GUIController.SetDeadScore(BattleContext.World.Points);
        StartCoroutine(DieProcess());
    }

    private IEnumerator DieProcess() {
        float a = 0;
        while (true) {
            BattleContext.GUIController.SetDeadPanelOpacity(a);
            a += 0.02f * 0.5f;
            if (a > 1) {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("BattleScene");
    }

	private void FixedUpdate() {
		UpdateMovement();
		m_hull.UpdateHull();
        m_hull.SetFlyingParameters(m_rigidbody.angularVelocity.y, m_shipParams.EnginePower < 450 ? 0 : m_power);
	}

	private void UpdateMovement() {
        // Rotation //
		float angleToTarget = AngleToNeedAngle;
		float longAngle = -Mathf.Sign(angleToTarget) * (360 - Mathf.Abs(angleToTarget));
		float actualAngle = angleToTarget;
		if ((m_rigidbody.angularVelocity.y * longAngle > 0) && (Mathf.Abs(m_rigidbody.angularVelocity.y) > 1)) {
			if (Mathf.Abs(m_rigidbody.angularVelocity.y * 50) > Mathf.Abs(longAngle + angleToTarget)) {
				actualAngle = longAngle;
			}
		}
		BattleContext.GUIController.SetRotationParams(m_neededAngle, actualAngle);
		float angularForce = Mathf.Sign(actualAngle) * Mathf.Sqrt(Mathf.Abs(actualAngle)) * m_shipParams.RotationPower;
		m_rigidbody.AddTorque(new Vector3(0, angularForce * m_rigidbody.mass * 0.02f, 0));
				
        // Velocity //
		float powerCoefficient = 0;
		if (m_power > 0) {
			powerCoefficient = 1;
		} else if (m_power < 0) {
			powerCoefficient = -1;
		}
		m_rigidbody.AddForce(m_power * LookVector * m_rigidbody.mass * 0.02f * m_shipParams.EnginePower);
		if (m_rigidbody.velocity.magnitude > 5) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
		}

        // Roll hull //
		m_hull.SetRollAngle(-m_rigidbody.angularVelocity.y * 15 * powerCoefficient);
	}

	public void SetAngle(float angle) {
		m_neededAngle = angle;
	}

	public void SetPower(float power) {
		m_power = Mathf.Clamp(power, -1, 1);
	}

	public void Charge() {
		if (m_effects.Stunned) {
			return;
		}
		if (!m_chargeSystem.InChargeTargeting) {
			return;
		}
		if (m_state == ShipState.InCharge) {
			return;
		}
		m_state = ShipState.InCharge;
		m_chargeSystem.Charge();
		BattleContext.World.SetTimeScaleMode(TimeScaleMode.SuperSlow);
		StartCoroutine(ChargeProcess());
	}

	private IEnumerator ChargeProcess() {
		float time = 0;
//		ParticleSystem[] pss = m_ps.GetComponentsInChildren<ParticleSystem>();
//		foreach (ParticleSystem system in pss) {
//			system.Clear();
//		}
//		m_ps.Simulate(Time.unscaledDeltaTime, true, true, false);
//		while (true) {
//			m_ps.Simulate(Time.unscaledDeltaTime, true, false, false);
//			time += Time.unscaledDeltaTime;
//			if (time >= 0.2) {
//				break;
//			}
//			yield return new WaitForEndOfFrame();
//		}
		float scale = 1.0f;
		while (true) {
			time += Time.unscaledDeltaTime;
			scale -= Time.unscaledDeltaTime * 10;
			m_hull.gameObject.transform.localScale = new Vector3(scale, scale, scale);
			if (scale <= 0.1f) {
				break;
			}
//			m_ps.Simulate(Time.unscaledDeltaTime, true, false, false);
			yield return new WaitForEndOfFrame();
		}
		m_hull.gameObject.SetActive(false);
//		while (true) {
//			time += Time.unscaledDeltaTime;
//			m_ps.Simulate(Time.unscaledDeltaTime, true, false, false);
//			if (time >= 1.0f) {
//				break;
//			}
//			yield return new WaitForEndOfFrame();
//		}
		m_chargeSystem.ChargeEffect.SetActive(true);
		ParticleSystem effectCharge = m_chargeSystem.ChargeEffect.GetComponent<ParticleSystem>();
		effectCharge.Clear();
		effectCharge.Simulate(Time.unscaledDeltaTime, true, true, false);
		time = 0;
		while (true) {
			time += Time.unscaledDeltaTime;
			transform.position += LookVector * Time.unscaledDeltaTime * 32;
			effectCharge.Simulate(Time.unscaledDeltaTime, true, false, false);
			if (time >= 0.5f) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		m_chargeSystem.ChargeEffect.SetActive(false);
		scale = 0.1f;
		m_hull.gameObject.SetActive(true);
		while (true) {
			transform.position += LookVector * Time.unscaledDeltaTime * 32;
			scale += Time.unscaledDeltaTime * 10;
			m_hull.gameObject.transform.localScale = new Vector3(scale, scale, scale);
			if (scale >= 1) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
		m_state = ShipState.OnMove;
	}

	private IEnumerator part() {
//		yield return new WaitForSecondsRealtime(0.25f);
		float time = 0;
		while (true) {
			m_ps.gameObject.transform.position = Position;
			m_ps.Simulate(Time.unscaledDeltaTime, true, false, true);
			time += Time.unscaledDeltaTime;
			if (time >= 0.2) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		float scale = 1.0f;
		while (true) {
			time += Time.unscaledDeltaTime;
			m_ps.gameObject.transform.position = Position;
			scale -= Time.unscaledDeltaTime * 10;
			m_hull.gameObject.transform.localScale = new Vector3(scale, scale, scale);
			if (scale <= 0.1f) {
				break;
			}
			m_ps.Simulate(Time.unscaledDeltaTime, true, false, true);
			yield return new WaitForEndOfFrame();
		}
		m_hull.gameObject.SetActive(false);
		while (true) {
			time += Time.unscaledDeltaTime;
			m_ps.Simulate(Time.unscaledDeltaTime, true, false, true);
			if (time >= 1.0f) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		GameObject charge = m_chargeSystem.ChargeEffect;
		charge.gameObject.SetActive(true);
		ParticleSystem effectCharge = charge.GetComponent<ParticleSystem>();
		TrailRenderer trail = charge.GetComponent<TrailRenderer>();
		List<IEnemyShip> ships = PlayerShipChargeSystem.GetTargets();
		bool left = true;
		foreach (IEnemyShip ship in ships) {
			effectCharge.Clear();
			trail.Clear();
			time = 0;
			Vector3 position = ship.Position - new Vector3(6, 0, 3);
			if (!left) {
				position = ship.Position - new Vector3(-6, 0, 3);
			} 
			
			while (true) {
				time += Time.unscaledDeltaTime;
				charge.transform.position = position;
				if (Vector3.Distance(position, ship.Position) < 1) {
					ship.Kill();
				}
				if (left) {
					position += new Vector3(1, 0, 0.5f) * Time.unscaledDeltaTime * 30;
				} else {
					position += new Vector3(-1, 0, 0.5f) * Time.unscaledDeltaTime * 30;
				}
				
				effectCharge.Simulate(Time.unscaledDeltaTime, true, false, true);
				if (time >= 0.5f) {
					left = !left;
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}
		charge.SetActive(false);
		time = 0;
		while (true) {
			time += Time.unscaledDeltaTime;
			transform.position += LookVector * Time.unscaledDeltaTime * 16;
			if (time >= 0.5f) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		scale = 0.1f;
		m_hull.gameObject.SetActive(true);
		while (true) {
			transform.position += LookVector * Time.unscaledDeltaTime * 16;
			scale += Time.unscaledDeltaTime * 10;
			m_hull.gameObject.transform.localScale = new Vector3(scale, scale, scale);
			if (scale >= 1) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		BattleContext.World.SetTimeScaleMode(TimeScaleMode.Normal);
	}

	public Vector3 LookVector {
		get {
			return new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		}
	}

	private float AngleToNeedAngle  {
		get {
			Vector3 vectorToTarget = new Vector3(Mathf.Cos(m_neededAngle * Mathf.PI / 180), 0, Mathf.Sin(m_neededAngle * Mathf.PI / 180));
			return MathHelper.AngleBetweenVectors(LookVector, vectorToTarget);
		}
	}

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

	private IEnumerator StunProcedure() {
		m_effects.Stunned = true;

		m_shipParams.RotationPower = 0;
		m_shipParams.EnginePower = 0;
		m_rigidbody.angularDrag = 0;

		m_stunFx.Play();
		yield return new WaitForSeconds(1.0f);
		m_stunFx.Stop();

		bool rotationWork = false;
		bool engineWork = false;
		while (!(rotationWork && engineWork)) {
			if (m_rigidbody.angularDrag < 19) {
				m_rigidbody.angularDrag += 1.0f;
			}
			if (m_shipParams.RotationPower < 70) {
				m_shipParams.RotationPower += 0.7f;
			} else {
				rotationWork = true;
			}
			if (m_shipParams.EnginePower < 900) {
				m_shipParams.EnginePower += 9.0f;
			} else {
				engineWork = true;
			}
			yield return new WaitForFixedUpdate();
		}
		m_rigidbody.angularDrag = 19;

		m_effects.Stunned = false;
	}

	private IEnumerator BashProcedure() {
		m_effects.Bashed = true;

		m_shipParams.RotationPower = 0;
		m_shipParams.EnginePower = 0;

		m_rigidbody.angularDrag = 0;
		yield return new WaitForSeconds(0.25f);
		
		bool rotationWork = false;
		bool engineWork = false;
		while (!(rotationWork && engineWork)) {
			if (m_rigidbody.angularDrag < 19) {
				m_rigidbody.angularDrag += 1.0f;
			}
			if (m_shipParams.RotationPower < 70) {
				m_shipParams.RotationPower += 1.4f;
			} else {
				rotationWork = true;
			}
			if (m_shipParams.EnginePower < 900) {
				m_shipParams.EnginePower += 18.0f;
			} else {
				engineWork = true;
			}
			yield return new WaitForFixedUpdate();
		}
		m_rigidbody.angularDrag = 19;

		m_effects.Bashed = false;
	}

}

public enum ShipState {
	OnMove,
	InCharge,
	Dead
}

public class EffectsOnShip {
	public bool Stunned { get; set; }
	public bool Bashed { get; set; }

	public EffectsOnShip() {
		Stunned = false;
		Bashed = false;
	}
}

public class ShipParams {
	public float EnginePower { get; set; }
	public float RotationPower { get; set; }

	public ShipParams() {
		EnginePower = 900;
		RotationPower = 70;
	}
}
