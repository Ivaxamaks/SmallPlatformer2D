using System;
using System.Collections.Generic;
using Common.Extentions;
using Common.Pool;
using Events;
using UniTaskPubSub;

namespace UI.HealthBar
{
    public class EnemyHealthBarManager : IDisposable
    {
        private readonly AsyncMessageBus _asyncMessageBus;
        private readonly MonoBehaviourPool<HealthBar> _healthBarPool;
        private CompositeDisposable _subscriptions;
        private Dictionary<Enemy.Enemy, HealthBar> _healthBars = new();

        public EnemyHealthBarManager(AsyncMessageBus asyncMessageBus, HealthBar healthBarPrefab)
        {
            _asyncMessageBus = asyncMessageBus;
            _healthBarPool = new MonoBehaviourPool<HealthBar>(healthBarPrefab, healthBarPrefab.transform.parent);
        }

        public void Initialize()
        {
            _subscriptions?.Dispose();
            _subscriptions = new CompositeDisposable()
            {
                _asyncMessageBus.Subscribe<EnemySpawnEvent>(eventData => OnSpawnHealthBar(eventData.Enemy)),
                _asyncMessageBus.Subscribe<EnemyDiedEvent>(eventData => OnDestroyHealthBar(eventData.Enemy))
            };
        }

        private void OnDestroyHealthBar(Enemy.Enemy enemy)
        {
            if(!_healthBars.ContainsKey(enemy)) return;
            _healthBars.Remove(enemy, out var healthBar);
            _healthBarPool.Release(healthBar);
        }

        private void OnSpawnHealthBar(Enemy.Enemy enemy)
        {
            var healthBar = _healthBarPool.Take();
            healthBar.Initialize(enemy.HealthController);
            _healthBars[enemy] = healthBar;
        }

        public void Dispose()
        {
            _healthBarPool.ReleaseAll();
            _subscriptions?.Dispose();
        }
    }
}