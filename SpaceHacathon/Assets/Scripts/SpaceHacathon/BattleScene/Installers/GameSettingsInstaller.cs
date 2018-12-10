using UnityEngine;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

        public override void InstallBindings() {
            
        }
        
    }

}