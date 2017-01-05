using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	private float m_neededAngle;
	private ThrottleState m_power;

	private ShipState m_state;
	private EffectsOnShip m_effects;
	private ShipParams m_shipParams;
	private ShipStatistics m_shipStatistics;

    [SerializeField]
    private PlayerShipHull m_hull;
	[SerializeField]
	private PlayerShipChargeSystem m_chargeSystem;
	[SerializeField]
	private ParticleSystem m_stunFx;

	private void Awake() {
		BattleContext.PlayerShip = this;

		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Initiate() {
		m_effects = new EffectsOnShip();
		m_shipParams = new ShipParams();
		m_shipStatistics = new ShipStatistics();

		m_hull.Initiate();
        m_chargeSystem.Initiate();

		m_state = ShipState.OnMove;
	}

	public void BlasterHit() {
		if (m_state != ShipState.OnMove) {
			return;
		}
		m_hull.Hit(0.1f);
		m_shipStatistics.StunHitEvent();
		StopCoroutine("StunProcedure");
		StartCoroutine(StunProcedure());
	}

	public void LaserHit() {
		if (m_state != ShipState.OnMove) {
			return;
		}
		m_hull.Hit(0.05f);
		m_shipStatistics.LaserHitEvent();
	}

	public void RocketHit(Vector3 position) {
		if (m_state != ShipState.OnMove) {
			return;
		}
		//hack
		position.y = 0;
		m_rigidbody.AddExplosionForce(m_rigidbody.mass * 50, Position + (position - Position).normalized * 3, 5);
		m_hull.Hit(0.5f);
		m_shipStatistics.RocketHitEvent();
		StopCoroutine("BashProcedure");
		StartCoroutine(BashProcedure());
	}

	public void EnemyShipHit() {
		switch (m_state) {
			case ShipState.OnMove:
				m_hull.Hit(10.0f);
				m_shipStatistics.EnemyShipHitEvent();
				break;
			case ShipState.InCharge:
				break;
		}
	}

	public void AddFuel() {
		m_chargeSystem.AddFuel();
	}

    public void Die() {
        if (m_state == ShipState.Dead) {
            return;
        }

		Analytics.CustomEvent("playerDied", new Dictionary<string, object> {
			{ "rocketHit", m_shipStatistics.RocketHit },
			{ "stunHit", m_shipStatistics.StunHit },
			{ "laserHit", m_shipStatistics.LaserHit },
			{ "enemyShipHit", m_shipStatistics.EnemyShipHit },
			{ "chargeUsed", m_shipStatistics.ChargeUsed }
		});

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
		if (m_state == ShipState.Dead) {
			if (m_rigidbody.velocity.magnitude > 0.05f) {
				m_rigidbody.velocity *= 0.95f;
			}
			if (m_rigidbody.angularVelocity.magnitude > 0.05f) {
				m_rigidbody.angularVelocity *= 0.95f;
			}
			return;
		}
		UpdateMovement();
		m_hull.UpdateHull();
        m_hull.SetFlyingParameters(m_rigidbody.angularVelocity.y, m_shipParams.EnginePower < 450  || m_state == ShipState.InCharge ? ThrottleState.Off : m_power);
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
		m_rigidbody.AddForce((int)m_power * LookVector * m_rigidbody.mass * 0.02f * m_shipParams.EnginePower);
		if (m_rigidbody.velocity.magnitude > 5) {
			m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
		}

        // Rotate hull //
		m_hull.SetAcceleration((LookVector * m_rigidbody.mass * 0.02f * m_shipParams.EnginePower).magnitude * (int)m_power);
		m_hull.SetRollAngle(-m_rigidbody.angularVelocity.y * 15 * powerCoefficient);
	}

	public void SetAngle(float angle) {
		if (m_state == ShipState.Dead) {
			return;
		}
		m_neededAngle = angle;
	}

	public void SetPower(ThrottleState power) {
		if (m_state == ShipState.Dead) {
			return;
		}
		m_power = power;
	}

	public void Charge() {
		if (m_state == ShipState.Dead) {
			return;
		}
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
		m_shipStatistics.UseChargeEvent();
		m_chargeSystem.Charge();
		StartCoroutine(ChargeProcess());
	}

	private IEnumerator ChargeProcess() {
		BattleContext.World.SetTimeScaleMode(TimeScaleMode.SuperSlow);
		float time = 0;
		float scale = 1.0f;
		while (true) {
			time += Time.unscaledDeltaTime;
			scale -= Time.unscaledDeltaTime * 10;
			m_hull.gameObject.transform.localScale = new Vector3(scale, scale, scale);
			if (scale <= 0.1f) {
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		m_hull.gameObject.SetActive(false);
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

public class ShipStatistics {
	private int m_rocketHit;
	private int m_stunHit;
	private int m_laserHit;
	private int m_enemyShipHit;
	private int m_chargeUsed;

	public ShipStatistics() {
		m_rocketHit = 0;
		m_stunHit = 0;
		m_laserHit = 0;
		m_enemyShipHit = 0;
	}

	public void RocketHitEvent() {
		m_rocketHit++;
	}

	public void StunHitEvent() {
		m_stunHit++;
	}

	public void LaserHitEvent() {
		m_laserHit++;
	}

	public void EnemyShipHitEvent() {
		m_enemyShipHit++;
	}

	public void UseChargeEvent() {
		m_chargeUsed++;
	}

	public int RocketHit {
		get {
			return m_rocketHit;
		}
	}

	public int StunHit {
		get {
			return m_stunHit;
		}
	}

	public int LaserHit {
		get {
			return m_laserHit;
		}
	}

	public int EnemyShipHit {
		get {
			return m_enemyShipHit;
		}
	}

	public int ChargeUsed {
		get {
			return m_chargeUsed;
		}
	}

}