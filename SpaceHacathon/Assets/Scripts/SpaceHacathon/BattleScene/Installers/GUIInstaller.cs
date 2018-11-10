using SpaceHacathon.BattleScene.GUI;
using SpaceHacathon.BattleScene.GUI.States;
using SpaceHacathon.Helpers.FSM;
using Zenject;

namespace SpaceHacathon.BattleScene.Installers {

    public class GUIInstaller : MonoInstaller {

        public override void InstallBindings() {
            Container.Bind<StateMachine<GUIStates, GUIEvents>>().WhenInjectedInto<GUIManager>().NonLazy();
            Container.Bind<StatesFactory<GUIStates, GUIEvents>>().WhenInjectedInto<StateMachine<GUIStates, GUIEvents>>();

            Container.Bind<IState<GUIStates, GUIEvents>>().To<PlayNormalState>().WhenInjectedInto<StatesFactory<GUIStates, GUIEvents>>();
            Container.Bind<IState<GUIStates, GUIEvents>>().To<InPauseState>().WhenInjectedInto<StatesFactory<GUIStates, GUIEvents>>();
            Container.Bind<IState<GUIStates, GUIEvents>>().To<PlayerDeadState>().WhenInjectedInto<StatesFactory<GUIStates, GUIEvents>>();
        }
    }

}