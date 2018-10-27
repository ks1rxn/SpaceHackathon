using SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Model;
using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Behaviours {

    public class AsteroidBlocksSpawner : IAsteroidBlocksSpawner {
        private AsteroidsBlock.Factory _blockFactory;
        
        [Inject]
        private void Construct(AsteroidsBlock.Factory blockFactory) {
            _blockFactory = blockFactory;
        }

        public AsteroidsBlock[,] SpawnBlocks(Transform parent) {
            AsteroidsBlock[,] blocks = new AsteroidsBlock[3, 3];
            
            for (int i = 0; i < 3; i++) {
                for (int g = 0; g < 3; g++) {
                    AsteroidsBlock block = CreateBlock(parent);
                    block.Initiate(new Vector3((i - 1) * 100, 0, (g - 1) * 100));
                    blocks[i, g] = block;
                }
            }

            return blocks;
        }
        
        private AsteroidsBlock CreateBlock(Transform parent) {
            AsteroidsBlock block = _blockFactory.Create("Prefabs/Asteroids/Block");
            block.transform.parent = parent;
            return block;
        }
        
    }

}