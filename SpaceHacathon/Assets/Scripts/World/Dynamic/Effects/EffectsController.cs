using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour {
	[SerializeField]
	private GameObject m_slowingCloudPrefab;

	private List<SlowingCloud> m_slowingClouds;

	public void Initiate() {
		m_slowingClouds = new List<SlowingCloud>();

		for (int i = 0; i != 2; i++) {
			CreateSlowingCloud();
		}
	}

	public void UpdateEntity() {
		for (int i = 0; i != m_slowingClouds.Count; i++) {
			if (m_slowingClouds[i].IsAlive) {
				m_slowingClouds[i].UpdateState();
			} 
		}
	}

	public SlowingCloud SpawnSlowingCloud(Vector3 position) {
		SlowingCloud targetSlowingCloud = null;
		foreach (SlowingCloud slowingCloud in m_slowingClouds) {
			if (!slowingCloud.IsAlive) {
				targetSlowingCloud = slowingCloud;
				break;
			}
		}
		if (targetSlowingCloud == null) {
			targetSlowingCloud = CreateSlowingCloud();
		}
		targetSlowingCloud.Spawn(position);
		return targetSlowingCloud;
	}

	private SlowingCloud CreateSlowingCloud() {
		SlowingCloud slowingCloud = (Instantiate(m_slowingCloudPrefab)).GetComponent<SlowingCloud>();
		slowingCloud.transform.parent = transform;
		slowingCloud.Initiate();
		m_slowingClouds.Add(slowingCloud);
		return slowingCloud;
	}

}
