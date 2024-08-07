using Common.StateMachine;
using Cysharp.Threading.Tasks;
using Enemy;
using Map;
using UI.HealthBar;

namespace States
{
    public class RunningGameState : IState
    {
        private readonly GameController _gameController;
        private readonly LastGameResultProvider _lastGameResultProvider;
        private readonly Player.Player _player;
        private readonly MapGenerator _mapGenerator;
        private readonly EnemySpawnController _enemySpawnController;
        private readonly EnemyHealthBarManager _healthBarManager;
        private StateMachine _stateMachine;

        public RunningGameState(GameController gameController,
            LastGameResultProvider lastGameResultProvider,
            Player.Player player,
            MapGenerator mapGenerator,
            EnemySpawnController enemySpawnController,
            EnemyHealthBarManager healthBarManager
            )
        {
            _gameController = gameController;
            _lastGameResultProvider = lastGameResultProvider;
            _player = player;
            _mapGenerator = mapGenerator;
            _enemySpawnController = enemySpawnController;
            _healthBarManager = healthBarManager;
        }
        
        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Enter()
        {
            _gameController.Initialize(OnEndGameEventHandler);
            _player.Initialize();
            _mapGenerator.Initialize();
            _healthBarManager.Initialize();
            _enemySpawnController.Initialize();
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            _gameController.Dispose();
            _enemySpawnController.Dispose();
            _healthBarManager.Dispose();
            _player.Dispose();
            return UniTask.CompletedTask;
        }

        private async void OnEndGameEventHandler(bool result)
        {
            _lastGameResultProvider.SetLastResult(result);
            await _stateMachine.Enter<EndGameState>();
        }
    }
}