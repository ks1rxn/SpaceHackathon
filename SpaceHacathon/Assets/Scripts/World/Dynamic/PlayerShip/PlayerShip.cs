using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
	private Rigidbody m_rigidbody;

	private float m_angle;
	private float m_power;

	private ShipState m_state;

    [SerializeField]
    private PlayerShipHull m_hull;
    [SerializeField]
    private PlayerShipChargeSystem m_chargeSystem;

	private float m_rotateForce;
	private float m_drag;
	private float m_moveForce;

	private const float DeltaTime = 0.02f;

	private void Awake() {
		BattleContext.PlayerShip = this;

        m_hull.Initiate();
        m_chargeSystem.Initiate();

		m_rigidbody = GetComponent<Rigidbody>();
		m_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		m_rotateForce = 70f;
		m_drag = 19;
		m_moveForce = 900;
	}

	private void OnCollisionEnter(Collision collision) {
   		if (m_state == ShipState.OnChargeFly) {
		    if (collision.gameObject.GetComponent<BlasterShip>() != null) {
		        BattleContext.World.AddPoints(30);
		    } else if (collision.gameObject.GetComponent<RocketShip>() != null) {
			    BattleContext.World.AddPoints(20);    
		    } else if (collision.gameObject.GetComponent<Rocket>() != null) {
		        BattleContext.World.AddPoints(40);  
		    }
   		    return;
		}
		if (collision.gameObject.GetComponent<ChargeFuel>() != null) {
			m_chargeSystem.AddFuel();
		} else if (collision.gameObject.GetComponent<Blaster>() != null) {
			m_hull.Hit(0.2f);
		} else if (collision.gameObject.GetComponent<Rocket>() != null) {
			m_rigidbody.AddExplosionForce(m_rigidbody.mass * 50, transform.position + (collision.transform.position - transform.position).normalized * 10, 20);
			m_rotateForce = 0;
			m_drag = 0;
			m_moveForce = 0;
			m_hull.Hit(0.5f);
		} else {
			m_hull.TakeDown();
		}
	}

    public void Die() {
        if (m_state == ShipState.Dead) {
            return;
        }
        m_state = ShipState.Dead;
        BattleContext.GUIController.SetDeadScore(BattleContext.World.Points);
        StartCoroutine(DieProcess());
    }

    private IEnumerator DieProcess() {
        float a = 0;
        while (true) {
            BattleContext.GUIController.SetDeadPanelOpacity(a);
            a += DeltaTime * 0.5f;
            if (a > 1) {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("BattleScene");
    }

	private void FixedUpdate() {
		if (m_rotateForce < 70) {
			m_rotateForce += DeltaTime * 70 / 2;
		}
		if (m_drag < 19) {
			m_drag += DeltaTime * 38 / 2;
		}
		if (m_moveForce < 900) {
			m_moveForce += DeltaTime * 900 / 2;
		}

		m_rigidbody.angularDrag = m_drag;
		UpdateMovement();

        m_hull.EngineSystem.SetFlyingParameters(m_state, m_rigidbody.angularVelocity.y, m_moveForce < 450 ? 0 : m_power);
		BattleContext.GUIController.SetRightJoystickAngle(m_angle);
	    BattleContext.GUIController.SetLeftJoysticValue(m_power);
	}

	private void UpdateMovement() {
		switch (m_state) {
			case ShipState.OnMove:
                // Rotation //
				float longAngle = -Mathf.Sign(AngleToTarget) * (360 - Mathf.Abs(AngleToTarget));
				float actualAngle = AngleToTarget;
				if ((m_rigidbody.angularVelocity.y * longAngle > 0) && (Mathf.Abs(m_rigidbody.angularVelocity.y) > 1)) {
				    if (Mathf.Abs(m_rigidbody.angularVelocity.y * 30) > Mathf.Abs(longAngle + AngleToTarget)) {
				        actualAngle = longAngle;
				    }
				}
				float angularForce = Mathf.Sign(actualAngle) * Mathf.Sqrt(Mathf.Abs(actualAngle)) * m_rotateForce;
				m_rigidbody.AddTorque(new Vector3(0, angularForce * m_rigidbody.mass * DeltaTime, 0));
				
                // Velocity //
				float powerCoefficient = 0;
				if (m_power > 0) {
				    powerCoefficient = 1;
				} else if (m_power < 0) {
				    powerCoefficient = -1;
				}
				m_rigidbody.AddForce(m_power * LookVector * m_rigidbody.mass * DeltaTime * m_moveForce);
				if (m_rigidbody.velocity.magnitude > 5) {
				    m_rigidbody.velocity = m_rigidbody.velocity.normalized * 5;
				}

                // Roll hull //
		        m_hull.Roll(-m_rigidbody.angularVelocity.y * 15 * powerCoefficient);
		 
				break;
		}
	}

	public void SetAngle(float angle) {
		switch (m_state) {
			case ShipState.OnMove:
				m_angle = angle;
				break;
			case ShipState.OnChargeTargeting:
				m_angle = angle;
				break;
		}
	}

	public void SetPower(float power) {
		power = Mathf.Clamp(power, -1, 1);
		switch (m_state) {
			case ShipState.OnMove:
				m_power = power;
				break;
		}
	}

	private float LookAngle {
		get {
			return -transform.rotation.eulerAngles.y;
		}
	}

	public Vector3 LookVector {
		get {
			return new Vector3(Mathf.Cos(-transform.rotation.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Sin(-transform.rotation.eulerAngles.y * Mathf.PI / 180));
		}
	}

	private float AngleToTarget  {
		get {
			Vector3 vectorToTarget = new Vector3(Mathf.Cos(m_angle * Mathf.PI / 180), 0, Mathf.Sin(m_angle * Mathf.PI / 180));
			return MathHelper.AngleBetweenVectors(LookVector, vectorToTarget);
		}
	}

}

public enum ShipState {
	OnMove = 0,
	TransferToChargeTargeting = 1,
	OnChargeTargeting = 2,
	OnChargeFly = 3,
	OnExitCharge = 4,
    Dead = 5
}
