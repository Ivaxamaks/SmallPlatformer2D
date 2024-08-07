using System;
using System.Threading;
using Common.Pool;
using Cysharp.Threading.Tasks;
using Player;
using Sounds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawnController : MonoBehaviour
    {
        public bool IsAllEnemiesSpawned => _spawnedEnemies == _totalEnemiesToSpawn;
        public int TotalEnemiesToSpawn => _totalEnemiesToSpawn;
    
        private GameSettings _gameSettings;
        private readonly Player.Player _player;
        private readonly SoundController _soundController;
        private CancellationTokenSource _cancellationTokenSource;
    
        private readonly MonoBehaviourPool<Enemy> _enemyPool;
        private readonly MonoBehaviourPool<AmmoPickup> _ammoPool;

        private int _totalEnemiesToSpawn;
        private int _spawnedEnemies;

        public EnemySpawnController(GameSettings gameSettings,
            EnemyFactory enemyFactory,
            Enemy enemyPrefab,
            AmmoPickup ammoPickupPrefab,
            Player.Player player,
            SoundController soundController)
        {
            _gameSettings = gameSettings;
            _player = player;
            _soundController = soundController;
            _enemyPool = new MonoBehaviourPool<Enemy>(enemyFactory.CreateEnemy, enemyPrefab.transform.parent);
            _ammoPool = new MonoBehaviourPool<AmmoPickup>(ammoPickupPrefab, ammoPickupPrefab.transform.parent);
        }

        public void Initialize()
        {
            _totalEnemiesToSpawn = Random.Range(_gameSettings.EnemySpawnAmountMin, _gameSettings.EnemySpawnAmountMax);
            SpawnEnemiesAsync().Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _enemyPool.ReleaseAll();
            _ammoPool.ReleaseAll();
        }

        private async UniTask SpawnEnemiesAsync()
        {
            _spawnedEnemies = 0;
            _cancellationTokenSource = new CancellationTokenSource();

            while (_spawnedEnemies < _totalEnemiesToSpawn)
            {
                SpawnEnemy();
                _spawnedEnemies++;
                await UniTask.Delay(TimeSpan.FromSeconds(_gameSettings.EnemySpawnCooldown),
                    cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private void SpawnEnemy()
        {
            var enemyType = (EnemyType)Random.Range(0, Enum.GetValues(typeof(EnemyType)).Length);
            var settings = _gameSettings.GetEnemySettingsByType(enemyType);
            var speed = Random.Range(settings.SpeedMin, settings.SpeedMax);
            var spawnDistance = Random.Range(settings.MinSpawnDistance, settings.MaxSpawnDistance);
            var spawnDirection = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
            var playerPosition = _player.transform.position;
            var spawnPosition = playerPosition + (Vector3)spawnDirection * spawnDistance;
            var enemy = _enemyPool.Take();
            enemy.transform.position = spawnPosition;
            enemy.Initialize(settings.Health, speed, settings.AnimatorController);
            enemy.OnEnemyDie += () => OnEnemyDie(enemy, settings.AmmoDropAmount);
            _soundController.PlaySound(SoundType.EnemySpawn);
        }

        private void OnEnemyDie(Enemy enemy, int settingsAmmoDropAmount)
        {
            _soundController.PlaySound(SoundType.EnemyDeath);
            var ammo = _ammoPool.Take();
            ammo.Initialize(settingsAmmoDropAmount);
            ammo.transform.position = enemy.transform.position + new Vector3(0,-1,0);
            _enemyPool.Release(enemy);
            ammo.OnDisposed += () => _ammoPool.Release(ammo);
        }
    }
}