using Common.StateMachine;
using States;
using VContainer.Unity;

public class GameStateMachine : StateMachine, IStartable, ITickable, IFixedTickable
{
    private readonly InputHandler.InputHandler _inputHandler;

    public GameStateMachine(BootstrapState bootstrapState,
        RunningGameState runningGameState,
        EndGameState endGameState,
        InputHandler.InputHandler inputHandler)  : base(bootstrapState, runningGameState, endGameState)
    {
        _inputHandler = inputHandler;
    }
    public async void Start()
    {
        await Enter<BootstrapState>();
    }

    public void Tick()
    {
        _inputHandler.Tick();
    }

    public void FixedTick()
    {
        _inputHandler.FixedTick();
    }
}