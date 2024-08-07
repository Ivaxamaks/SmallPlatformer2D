using Common.StateMachine;
using Cysharp.Threading.Tasks;
using Map;
using Sounds;
using UI;
using VContainer;

namespace States
{
    public class BootstrapState : IState
    {
        private readonly UIController _uiController;
        private readonly IObjectResolver _container;
        private readonly Player.Player _player;
        private readonly MapGenerator _mapGenerator;
        private readonly SoundController _soundController;
        private StateMachine _stateMachine;

        public BootstrapState(UIController uiController,
            IObjectResolver container,
            Player.Player player,
            MapGenerator mapGenerator,
            SoundController soundController)
        {
            _uiController = uiController;
            _container = container;
            _player = player;
            _mapGenerator = mapGenerator;
            _soundController = soundController;
        }

        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            _uiController.Initialize();
            _container.Inject(_player);
            _container.Inject(_mapGenerator);
            _container.Inject(_soundController);
            await _stateMachine.Enter<RunningGameState>();
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}