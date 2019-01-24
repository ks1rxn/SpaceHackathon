# SpaceHackathon

## Short Info
If you want to play this game by youself use this link:
<a href="https://play.google.com/store/apps/details?id=com.hardworkers.rockets">Hard Rockets on Google Play</a>

It's not a finished game. It is a prototype.
Codebase of this project looks a little messy. Development was started without any particular idea of how it should look ultimately.
Right after publishing gameplay prototype on Google Play development was being stopped.

Now it is used for experiments in program architecture. It is still WIP.

## Current architecture state.

Based on DI framework <a href="https://github.com/svermeulen/Zenject">Zenject</a>

Each object in game such as PlayerShip or Enemy is a small composition root and is set up by it's own installer.
```csharp
public class PlayerShipInstaller : MonoInstaller {
    [SerializeField]
    private GameObject _componentsHolder;

    public override void InstallBindings() {
        Container.Bind<PlayerShipController>().FromComponentOn(gameObject).AsSingle();

        InstallComponents();
        InstallFlyingNormalState();
        InstallStateMachine();
    }

    private void InstallComponents() {
        foreach (IComponent component in _componentsHolder.GetComponents<IComponent>()) {
            Container.Bind(component.GetType()).FromInstance(component).AsSingle();
        }
    }

    private void InstallFlyingNormalState() {
        Container.Bind<IBehaviour>().To<RotationBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
        Container.Bind<IBehaviour>().To<AccelerationBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
        Container.Bind<IBehaviour>().To<ConstraintsCheckingBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
        Container.Bind<IBehaviour>().To<EnginesVisualizationBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
        Container.Bind<IBehaviour>().To<RollHullBehaviour>().AsCached().WhenInjectedInto<FlyingNormalState>();
    }

    private void InstallStateMachine() {
        Container.Bind<StateMachine<PlayerShipStates, PlayerShipEvents>>().WhenInjectedInto<PlayerShipController>().NonLazy();
        Container.Bind<StatesFactory<PlayerShipStates, PlayerShipEvents>>().WhenInjectedInto<StateMachine<PlayerShipStates, PlayerShipEvents>>();

        Container.Bind<IState<PlayerShipStates, PlayerShipEvents>>().To<FlyingNormalState>().WhenInjectedInto<StatesFactory<PlayerShipStates, PlayerShipEvents>>();
        Container.Bind<IState<PlayerShipStates, PlayerShipEvents>>().To<DeadState>().WhenInjectedInto<StatesFactory<PlayerShipStates, PlayerShipEvents>>();
    }

}
```

It communicates with outer world using Facade. So objects coupling stays rather loose.
```csharp
public class PlayerShipFacade : MonoBehaviour, IPlayerControllable, ICameraTarget {
    private PlayerShipController _controller;
    private TransformComponent _transformComponent;
    private PhysicsComponent _physicsComponent;
    private RotationComponent _rotationComponent;
    private AccelerationComponent _accelerationComponent;

    [Inject]
    private void Construct(PlayerShipController controller, TransformComponent transformComponent, PhysicsComponent physicsComponent,
        RotationComponent rotationComponent, AccelerationComponent accelerationComponent) {

        _controller = controller;
        _transformComponent = transformComponent;
        _physicsComponent = physicsComponent;
        _rotationComponent = rotationComponent;
        _accelerationComponent = accelerationComponent;
    }

    public void SetDesiredAngle(float desiredAngle) {
        _rotationComponent.DesiredAngle = desiredAngle;
    }

    public void SetThrottle(ThrottleState throttle) {
        _accelerationComponent.ThrottleState = throttle;
    }

    public void Charge() {
        _controller.PushEvent(PlayerShipEvents.ChargePressed);
    }

    public PlayerShipOutput GetOutput() {
        PlayerShipOutput output = new PlayerShipOutput { DesiredAngle = _rotationComponent.DesiredAngle, RemainedAngleToDesired = _rotationComponent.RemainedAngleToDesired};
        return output;
    }

    public Vector3 Position => _transformComponent.Position;

    public Vector3 Velocity => _physicsComponent.Velocity;

}
```

In purpose not to violate DI principle, higher level modules such as GUI depend on interfaces such as IControllable which are implemented by lower level object like PlayerShip.
```csharp
public interface IPlayerControllable {

    void SetDesiredAngle(float desiredAngle);

    void SetThrottle(ThrottleState throttle);

    void Charge();

    PlayerShipOutput GetOutput();

}
```

Inside of object data and behaviour are separated. Data is stored in "Components" which are basically MonoBehaviours with seralized fields and small methods for their processing.
Behaviours are defined in "Behaviours". One or more components are injected into behaviour. The only job of behaviour is to change data in this components.
```csharp
public class RotationBehaviour : IBehaviour {
    private readonly TransformComponent _transformComponent;
    private readonly PhysicsComponent _physicsComponent;
    private readonly RotationComponent _rotationComponent;

    public RotationBehaviour(TransformComponent transformComponent, PhysicsComponent physicsComponent, RotationComponent rotationComponent) {
        _transformComponent = transformComponent;
        _physicsComponent = physicsComponent;
        _rotationComponent = rotationComponent;
    }

    public void Run() {
        float shortAngleToDesired = ShortAngleToDesired(_rotationComponent.DesiredAngle);
        float longAngleToDesired = LongAngleToDesired(shortAngleToDesired);
        float bestAngleForRotation = SelectBestAngleForRotation(shortAngleToDesired, longAngleToDesired, _physicsComponent.AngularVelocity);

        AddRotation(bestAngleForRotation);
        _rotationComponent.RemainedAngleToDesired = bestAngleForRotation;
    }

    private float ShortAngleToDesired(float desiredAngle)  {
        Vector3 vectorToTarget = new Vector3(Mathf.Cos(desiredAngle * Mathf.PI / 180), 0, Mathf.Sin(desiredAngle * Mathf.PI / 180));
        return MathHelper.AngleBetweenVectors(_transformComponent.LookVector, vectorToTarget);
    }

    private static float LongAngleToDesired(float shortAngleToDesired) {
        return -Mathf.Sign(shortAngleToDesired) * (360 - Mathf.Abs(shortAngleToDesired));
    }

    private static float SelectBestAngleForRotation(float shortAngle, float longAngle, Vector3 angularVelocity) {
        bool alreadyRotatingToLongSide = angularVelocity.y * longAngle > 0;
        bool rotationSpeedIsBigEnough = Mathf.Abs(angularVelocity.y) > 1;
        bool fasterToContinueRotation = Mathf.Abs(angularVelocity.y * 50) > Mathf.Abs(longAngle + shortAngle);

        if (alreadyRotatingToLongSide && rotationSpeedIsBigEnough && fasterToContinueRotation) {
            return longAngle;
        }

        return shortAngle;
    }

    private void AddRotation(float bestAngleForRotation) {
        const float rotationCoefficient = 1.0f;
        float angularForce = Mathf.Sign(bestAngleForRotation) * Mathf.Sqrt(Mathf.Abs(bestAngleForRotation)) * _rotationComponent.RotationPower * rotationCoefficient;
        const float slowing = 1.0f;
        _physicsComponent.AddTorque(new Vector3(0, angularForce * _physicsComponent.Mass * slowing * Time.fixedDeltaTime, 0));
    }
}
```
This organization reminds ECS paradigm, but is very simplified and component-system idea is used only to implement inside logic of objects, not throughout all code.
It is done this way because I think that separation of logic and data is very convinient way of code organization and help to implement SRP, but for small projects like this pure ECS is overengineering.

Internally a common game object is controlled by a pushdown automata <a href="http://gameprogrammingpatterns.com/state.html">Game Programming Patterns/State</a>  with event queue. All components are shared between all states.
But set of systems may be different for each state.

This is basic idea of architecture which is now in development. Despite there are some unresolved yet issues, generally it works fine.
Old version of code can be found outside of SpaceHackathon namespace.