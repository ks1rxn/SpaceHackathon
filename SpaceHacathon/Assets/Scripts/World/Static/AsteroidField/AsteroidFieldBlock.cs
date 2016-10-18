using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldBlock : MonoBehaviour {
	[SerializeField]
	private GameObject m_asteroidGroupPrefab;

	private List<AsteroidFieldBlockGroup> m_groups;

	private float size = 100;

	public void Initiate() {
		m_groups = new List<AsteroidFieldBlockGroup>();
		transform.localPosition = new Vector3();

		for (int i = 0; i != 50; i++) {
			Vector3 position = new Vector3((float) MathHelper.Random.NextDouble() * size - size / 2, -(float) MathHelper.Random.NextDouble() * 10, (float) MathHelper.Random.NextDouble() * size - size / 2);
			while (true) {
				bool tooClose = false;
				foreach (AsteroidFieldBlockGroup group in m_groups) {
					if (Vector3.Distance(group.transform.localPosition, position) < 8) {
						tooClose = true;
						break;
					}
				}
				if (!tooClose) {
					break;
				}
				position = new Vector3((float) MathHelper.Random.NextDouble() * size - size / 2, -(float) MathHelper.Random.NextDouble() * 10, (float) MathHelper.Random.NextDouble() * size - size / 2);
			}
			
			int r = MathHelper.Random.Next(100);
			if (r < 50) {
				AsteroidFieldBlockGroup group = CreateGroup();
				group.Initiate(AsteroidGroupType.Large, position);
				m_groups.Add(group);
			} else if (r < 80) {
				AsteroidFieldBlockGroup group = CreateGroup();
				group.Initiate(AsteroidGroupType.Medium, position);
				m_groups.Add(group);
			} else {
				AsteroidFieldBlockGroup group = CreateGroup();
				group.Initiate(AsteroidGroupType.Small, position);
				m_groups.Add(group);
			}
		}	
	}

	private AsteroidFieldBlockGroup CreateGroup() {
		GameObject go =  Instantiate(m_asteroidGroupPrefab);
		AsteroidFieldBlockGroup group = go.GetComponent<AsteroidFieldBlockGroup>();
		go.transform.parent = transform;
		return group;
	}

}
