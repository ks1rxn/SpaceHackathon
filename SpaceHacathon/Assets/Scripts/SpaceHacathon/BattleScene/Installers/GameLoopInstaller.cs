using System;
using SpaceHacathon.BattleScene.Game.Loop;
using SpaceHacathon.BattleScene.Game.Loop.States;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class GameLoopInstaller : Installer<GameLoopInstaller> {

        public override void InstallBindings() {
            Container.Bind(typeof(IGameLoop), typeof(IInitializable), typeof(ITickable), typeof(IDisposable)).To<GameLoop>().AsSingle();

            Container.Bind<StateMachine<GameLoopStates, GameLoopEvents>>().WhenInjectedInto<GameLoop>();
            Container.Bind<StatesFactory<GameLoopStates, GameLoopEvents>>().WhenInjectedInto<StateMachine<GameLoopStates, GameLoopEvents>>();
            
            Container.Bind<IState<GameLoopStates, GameLoopEvents>>().To<PlayNormalState>().WhenInjectedInto<StatesFactory<GameLoopStates, GameLoopEvents>>();
            Container.Bind<IState<GameLoopStates, GameLoopEvents>>().To<InPauseState>().WhenInjectedInto<StatesFactory<GameLoopStates, GameLoopEvents>>();
            
            Container.Bind<TimeSpeedController>().AsSingle();
            Container.Bind<ElapsedTimeCounter>().AsSingle();
        }
        
    }

}