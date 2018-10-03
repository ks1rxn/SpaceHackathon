using System.Collections.Generic;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Static.Asteroids {

	public class AsteroidsBlock : MonoBehaviour {
		[SerializeField]
		private GameObject _asteroidGroupPrefab;
		[SerializeField]
		private float _blockSize;

		private List<AsteroidsGroup> _groups;

		public void MoveTo(Vector3 position) {
			transform.localPosition = position;
		}

		public void Initiate() {
			_groups = new List<AsteroidsGroup>();
			transform.localPosition = new Vector3();

			for (int i = 0; i != 60; i++) {
				Vector3 position = GenerateRandomPosition();
				int tryCount = 0;
				while (true) {
					bool hasAnotherGroupNearby = false;
					foreach (AsteroidsGroup group in _groups) {
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
			Vector3 playerPosiion = BattleContext.BattleManager.Director.PlayerShip.Position;
			for (int i = 0; i != _groups.Count; i++) {
				Vector3 position = _groups[i].transform.position;
				if (position.z > playerPosiion.z - 10 && position.z < playerPosiion.z + 50 && position.x > playerPosiion.x - 50 && position.x < playerPosiion.x + 50) {
					_groups[i].UpdateRotations();
				}
			}
		}

		public bool IsShipOnBlock() {
			return GetShipPositionOnBlock() != ShipPositionOnBlock.NotOnBlock;
		}

		public ShipPositionOnBlock GetShipPositionOnBlock() {
			Vector3 shipPosition = BattleContext.BattleManager.Director.PlayerShip.Position;
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
				AsteroidsGroup group = CreateGroup();
				group.Initiate(AsteroidGroupType.Large, position);
			} else if (r < 80) {
				AsteroidsGroup group = CreateGroup();
				group.Initiate(AsteroidGroupType.Medium, position);
			} else {
				AsteroidsGroup group = CreateGroup();
				group.Initiate(AsteroidGroupType.Small, position);
			}
		}

		private Vector3 GenerateRandomPosition() {
			return new Vector3((float) MathHelper.Random.NextDouble() * _blockSize - _blockSize / 2, -(float) MathHelper.Random.NextDouble() * 10, (float) MathHelper.Random.NextDouble() * _blockSize - _blockSize / 2);
		}

		private AsteroidsGroup CreateGroup() {
			GameObject go =  Instantiate(_asteroidGroupPrefab);
			AsteroidsGroup group = go.GetComponent<AsteroidsGroup>();
			go.transform.parent = transform;
			_groups.Add(group);
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

}