using System.Collections.Generic;
using UnityEngine;

public class EffectsController : IController {
	[SerializeField]
	private GameObject m_slowingCloudPrefab;

	private List<SlowingCloud> m_slowingClouds;

	public override void Initiate() {
		m_slowingClouds = new List<SlowingCloud>();

		for (int i = 0; i != 2; i++) {
			CreateSlowingCloud();
		}
	}

	public override void FixedUpdateEntity() {
		for (int i = 0; i != m_slowingClouds.Count; i++) {
			if (m_slowingClouds[i].IsSpawned()) {
				m_slowingClouds[i].FixedUpdateEntity();
			} 
		}
	}

	public SlowingCloud SpawnSlowingCloud(Vector3 position) {
		SlowingCloud targetSlowingCloud = null;
		foreach (SlowingCloud slowingCloud in m_slowingClouds) {
			if (!slowingCloud.IsSpawned()) {
				targetSlowingCloud = slowingCloud;
				break;
			}
		}
		if (targetSlowingCloud == null) {
			targetSlowingCloud = CreateSlowingCloud();
		}
		targetSlowingCloud.Spawn(position, 0);
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
