using Enemy;
using Map;
using Player;
using Sounds;
using States;
using UI;
using UI.EndGamePanel;
using UI.HealthBar;
using UI.TopUIPanel;
using UniTaskPubSub;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private SoundListSO _soundsList;
    
    [SerializeField] private SoundController _soundController;
    [SerializeField] private EndGamePanelModel _endScreenPanelModel;
    [SerializeField] private TopUIPanelModel _topUIPanelModel;
    [SerializeField] private Player.Player _player;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private Enemy.Enemy _enemyPrefab;
    [SerializeField] private HealthBar _healthBarPrefab;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private AmmoPickup _ammoPickup;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_gameSettings);
        builder.RegisterInstance(_soundsList);
        builder.RegisterInstance(_player);
        builder.RegisterInstance(_enemyPrefab);
        builder.RegisterInstance(_healthBarPrefab);
        builder.RegisterInstance(_bulletPrefab);
        builder.RegisterInstance(_ammoPickup);
        builder.RegisterInstance(_endScreenPanelModel);
        builder.RegisterInstance(_topUIPanelModel);
        builder.RegisterInstance(_mapGenerator);
        builder.RegisterInstance(_soundController);
        
        builder.Register<BootstrapState>(Lifetime.Singleton);
        builder.Register<RunningGameState>(Lifetime.Singleton);
        builder.Register<EndGameState>(Lifetime.Singleton);
        
        builder.Register<GameController>(Lifetime.Singleton);
        builder.Register<AsyncMessageBus>(Lifetime.Singleton);
        builder.Register<EnemySpawnController>(Lifetime.Singleton);
        builder.Register<InputHandler.InputHandler>(Lifetime.Singleton);
        builder.Register<UIController>(Lifetime.Singleton);
        builder.Register<EnemyHealthBarManager>(Lifetime.Singleton);
        builder.Register<EnemyFactory>(Lifetime.Singleton);
        builder.Register<LastGameResultProvider>(Lifetime.Singleton);
        
        builder.RegisterEntryPoint<GameStateMachine>();
    }
}