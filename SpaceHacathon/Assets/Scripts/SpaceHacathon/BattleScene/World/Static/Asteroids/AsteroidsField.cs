using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Static.Asteroids {

	public class AsteroidsField : MonoBehaviour {
		private AsteroidsBlock[,] _blocks;
		private AsteroidsBlock.Factory _blockFactory;

		[Inject]
		private void Construct(AsteroidsBlock.Factory asteroidsBlockFactory) {
			_blockFactory = asteroidsBlockFactory;
			
			_blocks = new AsteroidsBlock[3, 3];

			for (int i = 0; i != 3; i++) {
				for (int g = 0; g != 3; g++) {
					AsteroidsBlock block = CreateBlock();
					block.Initiate();
					block.MoveTo(new Vector3((i - 1) * 100, 0, (g - 1) * 100));
					_blocks[i, g] = block;
				}
			}
		}

		public void FixedUpdate() {
			if (!_blocks[1, 1].IsShipOnBlock()) {
				FixBlocks();
			}
			ShipPositionOnBlock position = _blocks[1, 1].GetShipPositionOnBlock();
			if (position == ShipPositionOnBlock.NotOnBlock) {
				return;
			}
			switch (position) {
				case ShipPositionOnBlock.BottomLeft:
					_blocks[1, 1].UpdateGroups();
					_blocks[0, 1].UpdateGroups();
					_blocks[1, 0].UpdateGroups();
					_blocks[0, 0].UpdateGroups();
					break;
				case ShipPositionOnBlock.TopLeft:
					_blocks[1, 1].UpdateGroups();
					_blocks[0, 1].UpdateGroups();
					_blocks[0, 2].UpdateGroups();
					_blocks[1, 2].UpdateGroups();
					break;
				case ShipPositionOnBlock.TopRight:
					_blocks[1, 1].UpdateGroups();
					_blocks[1, 2].UpdateGroups();
					_blocks[2, 1].UpdateGroups();
					_blocks[2, 2].UpdateGroups();
					break;
				case ShipPositionOnBlock.BottomRight:
					_blocks[1, 1].UpdateGroups();
					_blocks[1, 0].UpdateGroups();
					_blocks[2, 1].UpdateGroups();
					_blocks[2, 0].UpdateGroups();
					break;
			}
		}

		private void FixBlocks() {
			if (_blocks[2, 1].IsShipOnBlock() || _blocks[2, 0].IsShipOnBlock() || _blocks[2, 2].IsShipOnBlock()) {
				SwapColumns(2, 1);
				SwapColumns(0, 2);
				FixColumnPosition(2);
			} else if (_blocks[0, 1].IsShipOnBlock() || _blocks[0, 0].IsShipOnBlock() || _blocks[0, 2].IsShipOnBlock()) {
				SwapColumns(0, 1);
				SwapColumns(0, 2);
				FixColumnPosition(0);
			}
			if (_blocks[1, 2].IsShipOnBlock() || _blocks[0, 2].IsShipOnBlock() || _blocks[2, 2].IsShipOnBlock()) {
				SwapRows(2, 1);
				SwapRows(0, 2);
				FixRowPosition(2);
			} else if (_blocks[1, 0].IsShipOnBlock() || _blocks[0, 0].IsShipOnBlock() || _blocks[2, 0].IsShipOnBlock()) {
				SwapRows(0, 1);
				SwapRows(0, 2);
				FixRowPosition(0);
			}
		}

		private void SwapColumns(int c1, int c2) {
			for (int i = 0; i != 3; i++) {
				AsteroidsBlock temp = _blocks[c1, i];
				_blocks[c1, i] = _blocks[c2, i];
				_blocks[c2, i] = temp;
			}
		}

		private void SwapRows(int r1, int r2) {
			for (int i = 0; i != 3; i++) {
				AsteroidsBlock temp = _blocks[i, r1];
				_blocks[i, r1] = _blocks[i, r2];
				_blocks[i, r2] = temp;
			}
		}

		private void FixColumnPosition(int c) {
			for (int i = 0; i != 3; i++) {
				Vector3 blockPosition = _blocks[c, i].transform.position;
				blockPosition.x = _blocks[1, 1].transform.position.x + (c > 1 ? 100 : -100);
				_blocks[c, i].transform.position = blockPosition;
			}
		}

		private void FixRowPosition(int r) {
			for (int i = 0; i != 3; i++) {
				Vector3 blockPosition = _blocks[i, r].transform.position;
				blockPosition.z = _blocks[1, 1].transform.position.z + (r > 1 ? 100 : -100);
				_blocks[i, r].transform.position = blockPosition;
			}
		}

		private AsteroidsBlock CreateBlock() {
			AsteroidsBlock block = _blockFactory.Create("Prefabs/Asteroids/Block");
			block.transform.parent = transform;
			return block;
		}

	}

}
