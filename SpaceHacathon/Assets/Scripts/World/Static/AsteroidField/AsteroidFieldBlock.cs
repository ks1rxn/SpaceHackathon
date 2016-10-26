﻿using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldBlock : MonoBehaviour {
	[SerializeField]
	private GameObject m_asteroidGroupPrefab;
	[SerializeField]
	private float m_blockSize;

	private List<AsteroidFieldBlockGroup> m_groups;

	public void MoveTo(Vector3 position) {
		transform.localPosition = position;
	}

	public void Initiate() {
		m_groups = new List<AsteroidFieldBlockGroup>();
		transform.localPosition = new Vector3();

		for (int i = 0; i != 60; i++) {
			Vector3 position = GenerateRandomPosition();
			int tryCount = 0;
			while (true) {
				bool hasAnotherGroupNearby = false;
				foreach (AsteroidFieldBlockGroup group in m_groups) {
					if (Vector3.Distance(group.transform.localPosition, position) < 8) {
						hasAnotherGroupNearby = true;
						break;
					}
				}
				if (!hasAnotherGroupNearby || tryCount > 100) {
					break;
				}
				tryCount++;
				position = GenerateRandomPosition();
			}

			CreateRandomGroup(position);

		}
	}

	public void UpdateGroups() {
		foreach (AsteroidFieldBlockGroup blockGroup in m_groups) {
			if (Vector3.Distance(blockGroup.transform.position, BattleContext.PlayerShip.transform.position) < 50) {
				blockGroup.UpdateRotations();
			}
		}
	}

	public bool IsShipOnBlock() {
		return GetShipPositionOnBlock() != ShipPositionOnBlock.NotOnBlock;
	}

	public ShipPositionOnBlock GetShipPositionOnBlock() {
		Vector3 shipPosition = BattleContext.PlayerShip.transform.position;
		if (shipPosition.x > transform.position.x + 50) {
			return ShipPositionOnBlock.NotOnBlock;
		}
		if (shipPosition.x < transform.position.x - 50) {
			return ShipPositionOnBlock.NotOnBlock;
		}
		if (shipPosition.z > transform.position.z + 50) {
			return ShipPositionOnBlock.NotOnBlock;
		}
		if (shipPosition.z < transform.position.z - 50) {
			return ShipPositionOnBlock.NotOnBlock;
		}
		if (shipPosition.x > transform.position.x) {
			if (shipPosition.z > transform.position.z) {
				return ShipPositionOnBlock.TopRight;
			}
			return ShipPositionOnBlock.BottomRight;
		}
		if (shipPosition.z > transform.position.z) {
			return ShipPositionOnBlock.TopLeft;
		}
		return ShipPositionOnBlock.BottomLeft;
	}

	private void CreateRandomGroup(Vector3 position) {
		int r = MathHelper.Random.Next(100);
		if (r < 50) {
			AsteroidFieldBlockGroup group = CreateGroup();
			group.Initiate(AsteroidGroupType.Large, position);
		} else if (r < 80) {
			AsteroidFieldBlockGroup group = CreateGroup();
			group.Initiate(AsteroidGroupType.Medium, position);
		} else {
			AsteroidFieldBlockGroup group = CreateGroup();
			group.Initiate(AsteroidGroupType.Small, position);
		}
	}

	private Vector3 GenerateRandomPosition() {
		return new Vector3((float) MathHelper.Random.NextDouble() * m_blockSize - m_blockSize / 2, -(float) MathHelper.Random.NextDouble() * 10, (float) MathHelper.Random.NextDouble() * m_blockSize - m_blockSize / 2);
	}

	private AsteroidFieldBlockGroup CreateGroup() {
		GameObject go =  Instantiate(m_asteroidGroupPrefab);
		AsteroidFieldBlockGroup group = go.GetComponent<AsteroidFieldBlockGroup>();
		go.transform.parent = transform;
		m_groups.Add(group);
		return group;
	}

}

public enum ShipPositionOnBlock {
	NotOnBlock = - 1,
	TopLeft = 0,
	TopRight = 1,
	BottomRight = 2,
	BottomLeft = 3
}