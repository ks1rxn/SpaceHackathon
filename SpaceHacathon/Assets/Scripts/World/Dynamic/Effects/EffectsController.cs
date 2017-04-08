using System.Collections.Generic;
using UnityEngine;

public class EffectsController : IController {
	private List<SlowingCloud> m_slowingClouds;

	public override void Initiate() {
		m_slowingClouds = new List<SlowingCloud>();

		for (int i = 0; i != 2; i++) {
			EntitiesHelper.CreateEntity(AvailablePrefabs.SlowingCloud, gameObject, m_slowingClouds);
		}
	}

	public override void FixedUpdateEntity() {
		for (int i = 0; i != m_slowingClouds.Count; i++) {
			if (m_slowingClouds[i].IsSpawned()) {
				m_slowingClouds[i].FixedUpdateEntity();
			} 
		}
	}

	public void SpawnSlowingCloud(Vector3 position) {
		EntitiesHelper.SpawnEntity<SlowingCloud>(AvailablePrefabs.SlowingCloud, gameObject, m_slowingClouds, position, 0);
	}

}
