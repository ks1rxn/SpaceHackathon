using SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Model;
using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Behaviours {

    public class AsteroidBlocksFixer : IAsteroidBlocksFixer {
        private AsteroidsBlock[,] _blocks;
    
        public AsteroidsBlock[,] FixBlocks(AsteroidsBlock[,] blocks) {
            _blocks = blocks;
            
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

            return _blocks;
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
        
    }

}