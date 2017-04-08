using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour {

	[SerializeField]
	private List<ListedPrefab> m_prefabs;
	private IDictionary<AvailablePrefabs, GameObject> m_actualPrefabs;

	public void Initiate() {
		m_actualPrefabs = new Dictionary<AvailablePrefabs, GameObject>(m_prefabs.Count);
		foreach (ListedPrefab prefab in m_prefabs) {
			m_actualPrefabs.Add(prefab.name, prefab.prefab);
		}
	}

	public GameObject GetByType(AvailablePrefabs type) {
		if (m_actualPrefabs == null) {
			return null;
		}
		GameObject target;
		if (m_actualPrefabs.TryGetValue(type, out target)) {
			return target;
		}
		return null;
	}

}

[Serializable]
 public struct ListedPrefab {
     public AvailablePrefabs name;
     public GameObject prefab;
 }

public enum AvailablePrefabs {

	// Ships //

	PlayerShip,
	DroneCarrier,
	StunShip,
	RocketShip,
	RamShip,
	SpaceMine,
	
	// Projectiles //

	Missile,
	StunProjectile,
	CarrierRocket,
	Laser,

	//Explosions //

	ShipExplosion,
	RocketExplosion,
	LaserExplosion,
	MineExplosion,

	// Effects //

	SlowingCloud,

	// Allies //

	HealDroneStation,
	HealDrone,

	// Bonuses //

	ChargeFuel,
	TimeBonus

}
