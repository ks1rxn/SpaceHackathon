using UnityEngine;

namespace SpaceHacathon.BattleScene.World.Dynamic.Camera {

    public interface ICameraTarget {
        
        Vector3 Position { get; }
        
        Vector3 Speed { get; }

    }

}