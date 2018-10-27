using SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Model;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt {

	public class AsteroidsField : MonoBehaviour {
		private IAsteroidBlocksSpawner _blocksSpawner;
		private IAsteroidBlocksFixer _blocksFixer;
		private AsteroidsBlock[,] _blocks;

		[Inject]
		private void Construct(IAsteroidBlocksSpawner blocksSpawner, IAsteroidBlocksFixer blocksFixer) {
			_blocksSpawner = blocksSpawner;
			_blocksFixer = blocksFixer;
		}

		private void Start() {
			_blocks = _blocksSpawner.SpawnBlocks(transform);
		}

		public void FixedUpdate() {
			if (!_blocks[1, 1].IsShipOnBlock()) {
				_blocks = _blocksFixer.FixBlocks(_blocks);
			}
			ShipPositionOnBlock position = _blocks[1, 1].GetShipPositionOnBlock();
			if (position == ShipPositionOnBlock.NotOnBlock) {
				return;
			}
			_blocks[1, 1].UpdateGroups();
			switch (position) {
				case ShipPositionOnBlock.BottomLeft:
					_blocks[0, 1].UpdateGroups();
					_blocks[1, 0].UpdateGroups();
					_blocks[0, 0].UpdateGroups();
					break;
				case ShipPositionOnBlock.TopLeft:
					_blocks[0, 1].UpdateGroups();
					_blocks[0, 2].UpdateGroups();
					_blocks[1, 2].UpdateGroups();
					break;
				case ShipPositionOnBlock.TopRight:
					_blocks[1, 2].UpdateGroups();
					_blocks[2, 1].UpdateGroups();
					_blocks[2, 2].UpdateGroups();
					break;
				case ShipPositionOnBlock.BottomRight:
					_blocks[1, 0].UpdateGroups();
					_blocks[2, 1].UpdateGroups();
					_blocks[2, 0].UpdateGroups();
					break;
			}
		}

	}

}
