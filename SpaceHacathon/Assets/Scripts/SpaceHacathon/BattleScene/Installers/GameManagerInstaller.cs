using System;
using SpaceHacathon.BattleScene.Game.Manager;
using SpaceHacathon.BattleScene.Game.Manager.States;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class GameManagerInstaller : Installer<GameManagerInstaller> {

        public override void InstallBindings() {
            Container.BindInterfacesTo<GameManager>().AsSingle();

            Container.Bind<StateMachine<GameManagerStates, GameManagerEvents>>().WhenInjectedInto<GameManager>();
            Container.Bind<StatesFactory<GameManagerStates, GameManagerEvents>>().WhenInjectedInto<StateMachine<GameManagerStates, GameManagerEvents>>();
            
            Container.Bind<IState<GameManagerStates, GameManagerEvents>>().To<PlayNormalState>().WhenInjectedInto<StatesFactory<GameManagerStates, GameManagerEvents>>();
            Container.Bind<IState<GameManagerStates, GameManagerEvents>>().To<InPauseState>().WhenInjectedInto<StatesFactory<GameManagerStates, GameManagerEvents>>();
            
            Container.Bind<TimeSpeedController>().AsSingle();
            Container.Bind<ElapsedTimeCounter>().AsSingle();
        }
        
    }

}