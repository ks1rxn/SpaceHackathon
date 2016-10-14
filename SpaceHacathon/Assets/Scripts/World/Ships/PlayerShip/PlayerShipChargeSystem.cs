using UnityEngine;

public class PlayerShipChargeSystem : MonoBehaviour {
    [SerializeField]
	private ParticleSystem m_chargeStartEffect;
	[SerializeField]
	private Transform m_chargeOwn;
	[SerializeField]
	private Transform m_chargeTarget;

    private float m_chargeTargetingMinTime;
    private float m_chargeTime;

    private float m_chargeFuel;
    private bool m_chargeTargeting;

    public void Initiate() {
        StopChargeTargeting();
        m_chargeFuel = 1;
    }

    public void PlayStartCharge(float angle) {
        m_chargeStartEffect.startRotation = -angle * Mathf.PI / 180;
		m_chargeStartEffect.Play();
    }

    public void StartChargeTargeting() {
        m_chargeOwn.gameObject.SetActive(true);
		m_chargeTarget.gameObject.SetActive(true);

        m_chargeTargetingMinTime = 0.1f;
        m_chargeFuel -= 0.3f;
        m_chargeTargeting = true;
    }

    public void StopChargeTargeting() {
        m_chargeOwn.gameObject.SetActive(false);
		m_chargeTarget.gameObject.SetActive(false);

        m_chargeTargeting = false;
    }

    public void RotateChargeTarget(float angle) {
        m_chargeTarget.rotation = new Quaternion();
		m_chargeTarget.Rotate(new Vector3(0, 1, 0), angle);
    }

    private void Update() {
        if (m_chargeTargeting) {
            m_chargeFuel -= 0.2f * Time.deltaTime;
        } else {
            m_chargeFuel += 0.04f * Time.deltaTime;
        }
        m_chargeFuel = Mathf.Clamp(m_chargeFuel, 0, 1);
        BattleContext.GUIController.SetCharge(m_chargeFuel);
    }

    public float ChargeFuel {
        get {
            return m_chargeFuel;
        }
    }

    public float ChargeTargetingMinTime {
        get {
            return m_chargeTargetingMinTime;
        }
        set {
            m_chargeTargetingMinTime = value;
        }
    }

    public float ChargeTime {
        get {
            return m_chargeTime;
        }
        set {
            m_chargeTime = value;
        }
    }
}
