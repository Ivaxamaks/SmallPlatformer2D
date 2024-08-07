using System;
using System.Threading;
using Common.Extentions;
using Cysharp.Threading.Tasks;
using Enemy;
using Events;
using UniTaskPubSub;

public class GameController
{
    private readonly AsyncMessageBus _asyncMessageBus;
    private readonly EnemySpawnController _enemySpawnController;
    private CompositeDisposable _subscriber;
    private Action<bool> _winLooseCallback;
    private int _enemyDiedCount;

    public GameController(AsyncMessageBus asyncMessageBus,
        EnemySpawnController enemySpawnController)
    {
        _asyncMessageBus = asyncMessageBus;
        _enemySpawnController = enemySpawnController;
    }

    public void Initialize(Action<bool> callback)
    {
        _enemyDiedCount = 0;
        _winLooseCallback = callback;
        _subscriber = new CompositeDisposable();
        _subscriber.Add(_asyncMessageBus.Subscribe<EnemyDiedEvent>(OnEnemyDiedEventHandler));
        _subscriber.Add(_asyncMessageBus.Subscribe<PlayerDiedEvent>(OnPlayerDiedEventHandler));
        _subscriber.Add(_asyncMessageBus.Subscribe<PlayerAmmoAmountChangedEvent>(PlayerAmmoWasChangedEventHandler));
    }

    public void Dispose()
    {
        _subscriber.Dispose();
    }

    private void OnPlayerDiedEventHandler(PlayerDiedEvent eventData)
    {
        _winLooseCallback?.Invoke(false);
    }

    private void PlayerAmmoWasChangedEventHandler(PlayerAmmoAmountChangedEvent eventData)
    {
        if(eventData.AmmoCount > 0) return;
        _winLooseCallback?.Invoke(false);
    }

    private void OnEnemyDiedEventHandler(EnemyDiedEvent eventData)
    {
        _enemyDiedCount++;
        if(_enemySpawnController.IsAllEnemiesSpawned && _enemySpawnController.TotalEnemiesToSpawn == _enemyDiedCount)
            _winLooseCallback?.Invoke(true);
    }
}