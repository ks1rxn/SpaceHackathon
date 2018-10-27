using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Static.AsteroidBelt.Model {

    public interface IAsteroidBlocksSpawner {
        AsteroidsBlock[,] SpawnBlocks(Transform parent);
    }

}