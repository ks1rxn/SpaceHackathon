using System.Collections.Generic;
using UnityEngine;

class EntitiesHelper {

	public static IEntity CreateEntity(AvailablePrefabs prefab, GameObject parent) {
		IEntity entity = Object.Instantiate(BattleContext.BattleManager.PrefabsManager.GetByType(prefab)).GetComponent<IEntity>();
		entity.transform.parent = parent.transform;
		entity.Initiate();
		return entity;
	}

	public static IEntity CreateEntity<T1>(AvailablePrefabs prefab, GameObject parent, List<T1> list) where T1 : IEntity {
		IEntity entity = CreateEntity(prefab, parent);
		if (list != null) {
			list.Add((T1) entity);
		}
		return entity;
	}

	public static IEntity CreateEntity<T1, T2>(AvailablePrefabs prefab, GameObject parent, List<T1> list, List<T2> list2 ) where T1 : IEntity where T2 : IEntity {
		IEntity entity = CreateEntity(prefab, parent, list);
		if (list2 != null) {
			list2.Add((T2) entity);
		}
		return entity;
	}

	public static ISpawnable SpawnEntity<T1, T2>(AvailablePrefabs prefab, GameObject parent, List<T1> searchList, List<T2> helperList, Vector3 position, float rotation) where T1 : ISpawnable where T2 : ISpawnable {
		ISpawnable target = null;
		foreach (T1 spawnable in searchList) {
			if (!spawnable.IsSpawned()) {
				target = spawnable;
				break;
			}
		}
		if (target == null) {
			target = (ISpawnable) CreateEntity(prefab, parent, searchList, helperList);
		}
		target.Spawn(position, rotation);
		return target;
	}

}