using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	private float m_neededAngle;
	private float m_power;

	private ShipState m_state;
	private EffectsOnShip m_effects;
	private ShipMovementSystem m_shipMovementSystem;

    [SerializeField]
    private PlayerShipHull m_hull;
    [SerializeField]
    private PlayerShipChargeSystem m_chargeSystem;
	[SerializeField]
	private GameObject m_chargeEffect;
	[SerializeField]
	private ParticleSystem m_stunFx;

	private void Awake() {
		BattleContext.PlayerShip = this;

        m_hull.Initiate();
        m_chargeSystem.Initiate();

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		m_state = ShipState.OnMove;
		m_effects = new EffectsOnShip();
		m_shipMovementSystem = new ShipMovementSystem();
	}

	private void OnTriggerEnter(Collider other) { 
		if (other.gameObject.GetComponent<ChargeFuel>() != null) {
			m_chargeSystem.AddFuel();
		} else if (other.gameObject.GetComponent<Blaster>() != null) {
			Stun();
			m_hull.Hit(0.1f);
		} else if (other.gameObject.GetComponent<Rocket>() != null) {
			//hack
			Vector3 collisionPos = other.transform.position;
			collisionPos.y = 0;
			m_rigidbody.AddExplosionForce(m_rigidbody.mass * 50, transform.position + (collisionPos - transform.position).normalized * 3, 5);
			Bash();
			m_hull.Hit(0.5f);
		} else {
			m_hull.Hit(100);
		}
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

	private void Stun() {
		StopCoroutine("StunProcedure");
		StartCoroutine(StunProcedure());
	}

	private void Bash() {
		StopCoroutine("BashProcedure");
		StartCoroutine(BashProcedure());
	}

	private void FixedUpdate() {
		m_hull.UpdateHull();

		UpdateMovement();

        m_hull.SetFlyingParameters(m_rigidbody.angularVelocity.y, m_shipMovementSystem.EnginePower < 450 ? 0 : m_power);
	}

	private void UpdateMovement() {
		switch (m_state) {
			case ShipState.OnMove:
                // Rotation //
				float angleToTarget = AngleToNeedAngle;
				float longAngle = -Mathf.Sign(angleToTarget) * (360 - Mathf.Abs(angleToTarget));
				float actualAngle = angleToTarget;
				if ((m_rigidbody.angularVelocity.y * longAngle > 0) && (Mathf.Abs(m_rigidbody.angularVelocity.y) > 1)) {
				    if (Mathf.Abs(m_rigidbody.angularVelocity.y * 30) > Mathf.Abs(longAngle + angleToTarget)) {
				        actualAngle = longAngle;
				    }
				}
				float angularForce = Mathf.Sign(actualAngle) * Mathf.Sqrt(Mathf.Abs(actualAngle)) * m_shipMovementSystem.RotationPower;
				m_rigidbody.AddTorque(new Vector3(0, angularForce * m_rigidbody.mass * 0.02f, 0));
				
                // Velocity //
				float powerCoefficient = 0;
				if (m_power > 0) {
				    powerCoefficient = 1;
				} else if (m_power < 0) {
				    powerCoefficient = -1;
				}
				m_rigidbody.AddForce(m_power * LookVector * m_rigidbody.mass * 0.02f * m_shipMovementSystem.EnginePower);
				if (m_rigidbody.velocity.magnitude > 5) {
				    m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
				}

                // Roll hull //
		        m_hull.SetRollAngle(-m_rigidbody.angularVelocity.y * 15 * powerCoefficient);
		 
				break;
		}
	}

	public void Charge() {
		if (!m_chargeSystem.InChargeTargeting) {
			return;
		}
//		BattleContext.World.SetTimeScaleMode(TimeScaleMode.Stoped);
		m_state = ShipState.InCharge;
		m_chargeEffect.SetActive(true);
		StartCoroutine(ChargeEffect());
//		m_chargeSystem.Charge();
//		transform.position += LookVector * 8;
//		m_rigidbody.angularVelocity = new Vector3();
//		m_rigidbody.velocity = LookVector * 10;
//		m_state = ShipState.OnMove;
	}

	private IEnumerator ChargeEffect() {
		Vector3 position = transform.position + new Vector3(-10, 0, -8);
		Vector3 dir = new Vector3(1, 0, 0.2f).normalized;
		m_chargeEffect.transform.position = position;
		for (int i = 0; i != 10; i++) {
			position += dir * 2f;
			m_chargeEffect.transform.position = position;
			yield return new WaitForFixedUpdate();
		}
		m_chargeEffect.GetComponent<TrailRenderer>().Clear();
		position = transform.position + new Vector3(20, 0, -1);
		dir = new Vector3(-1, 0, 0.2f).normalized;
		m_chargeEffect.transform.position = position;
		for (int i = 0; i != 100; i++) {
			position += dir * 2f;
			m_chargeEffect.transform.position = position;
			yield return new WaitForFixedUpdate();
		}
		m_chargeEffect.SetActive(false);
	}

	public void SetAngle(float angle) {
		m_neededAngle = angle;
	}

	public void SetPower(float power) {
		m_power = Mathf.Clamp(power, -1, 1);
	}

	public void AddFuel() {
		m_chargeSystem.AddFuel();
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

		m_shipMovementSystem.RotationPower = 0;
		m_shipMovementSystem.EnginePower = 0;
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
			if (m_shipMovementSystem.RotationPower < 70) {
				m_shipMovementSystem.RotationPower += 0.7f;
			} else {
				rotationWork = true;
			}
			if (m_shipMovementSystem.EnginePower < 900) {
				m_shipMovementSystem.EnginePower += 9.0f;
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

		m_shipMovementSystem.RotationPower = 0;
		m_shipMovementSystem.EnginePower = 0;

		m_rigidbody.angularDrag = 0;
		yield return new WaitForSeconds(0.25f);
		
		bool rotationWork = false;
		bool engineWork = false;
		while (!(rotationWork && engineWork)) {
			if (m_rigidbody.angularDrag < 19) {
				m_rigidbody.angularDrag += 1.0f;
			}
			if (m_shipMovementSystem.RotationPower < 70) {
				m_shipMovementSystem.RotationPower += 1.4f;
			} else {
				rotationWork = true;
			}
			if (m_shipMovementSystem.EnginePower < 900) {
				m_shipMovementSystem.EnginePower += 18.0f;
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

public class ShipMovementSystem {
	public float EnginePower { get; set; }
	public float RotationPower { get; set; }

	public ShipMovementSystem() {
		EnginePower = 900;
		RotationPower = 70;
	}
}
