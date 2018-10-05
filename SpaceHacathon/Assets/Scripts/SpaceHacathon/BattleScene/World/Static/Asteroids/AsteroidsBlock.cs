using System.Collections.Generic;
using SpaceHacathon.Helpers;
using UnityEngine;
using Random = System.Random;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Static.Asteroids {

	public class AsteroidsBlock : MonoBehaviour {
		[SerializeField]
		private float _blockSize;

		private List<AsteroidsGroup> _groups;
		private AsteroidsGroup.Factory _groupFactory;
		private Random _random;

		[Inject]
		private void Construct(AsteroidsGroup.Factory groupFactory, Random random) {
			_groupFactory = groupFactory;
			_random = random;
		}

		public void Initiate(Vector3 position) {
			_groups = new List<AsteroidsGroup>();
			transform.localPosition = position;

			for (int i = 0; i != 60; i++) {
				Vector3 newGroupPosition = GenerateRandomPosition();
				int tryCount = 0;
				while (true) {
					bool hasAnotherGroupNearby = false;
					foreach (AsteroidsGroup group in _groups) {
						if (Vector3.Distance(group.transform.localPosition, newGroupPosition) < 8) {
							hasAnotherGroupNearby = true;
							break;
						}
					}
					if (!hasAnotherGroupNearby || tryCount > 100) {
						break;
					}
					tryCount++;
					newGroupPosition = GenerateRandomPosition();
				}
				CreateRandomGroup(newGroupPosition);
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
			int r = _random.Next(100);
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
			AsteroidsGroup group = _groupFactory.Create("Prefabs/Asteroids/Group");
			group.transform.parent = transform;
			_groups.Add(group);
			return group;
		}

		public class Factory : PlaceholderFactory<string, AsteroidsBlock> { }

	}

	public enum ShipPositionOnBlock {
		NotOnBlock = - 1,
		TopLeft = 0,
		TopRight = 1,
		BottomRight = 2,
		BottomLeft = 3
	}

}