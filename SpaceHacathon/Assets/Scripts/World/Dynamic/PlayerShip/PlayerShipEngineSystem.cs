using UnityEngine;

public class PlayerShipEngineSystem : MonoBehaviour {
    [SerializeField]
    private ParticleSystem[] m_engines;
    private bool[] m_engineState;

    public void Initiate() {
        m_engineState = new bool[m_engines.Length];
    }

    public void SetFlyingParameters(float rotation, float enginePower) {
        float powerCoef = 1;
		if (enginePower < -0.1f) {
			powerCoef = -1;
		}
        if (Mathf.Abs(rotation) > 0.5f) {
			if (rotation * powerCoef < 0) {
				SetEngineState(3, true);
				SetEngineState(6, true);
				SetEngineState(4, false);
				SetEngineState(5, false);
			} else {
				SetEngineState(3, false);
				SetEngineState(6, false);
				SetEngineState(4, true);
				SetEngineState(5, true);
			}
		} else {
			SetEngineState(3, false);
			SetEngineState(6, false);
			SetEngineState(4, false);
			SetEngineState(5, false);
		}
		if (enginePower > 0.1f) {
			SetEngineState(0, true);
			SetEngineState(1, true);
			SetEngineState(2, true);
			SetEngineState(7, false);
			SetEngineState(8, false);
		} else if (enginePower < -0.1f) {
			SetEngineState(0, false);
			SetEngineState(1, false);
			SetEngineState(2, false);
			SetEngineState(7, true);
			SetEngineState(8, true);
		} else {
			SetEngineState(0, false);
			SetEngineState(1, false);
			SetEngineState(2, false);
			SetEngineState(7, false);
			SetEngineState(8, false);
		}
    }

    private void SetEngineState(int engine, bool state) {
		if (m_engineState[engine] != state) {
			m_engineState[engine] = state;
			if (state) {
				m_engines[engine].Play();
			} else {
				m_engines[engine].Stop();
			}
		}
	}

}
