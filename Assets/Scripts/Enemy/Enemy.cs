using System;
using Events;
using JetBrains.Annotations;
using UniTaskPubSub;
using UnityEngine;
using VContainer;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public Action OnEnemyDie;
    
        public EnemyHealthController HealthController => _enemyHealthController;

        [SerializeField, NotNull] private Animator _animator;
        [SerializeField, NotNull] private EnemyHealthController _enemyHealthController;
        [SerializeField, NotNull] private Rigidbody2D _rigidbody2D;

        private Player.Player _player;
        private AsyncMessageBus _asyncMessageBus;

        private float _speed;

        [Inject]
        public void Construct(AsyncMessageBus asyncMessageBus,
            Player.Player player)
        {
            _asyncMessageBus = asyncMessageBus;
            _player = player;
        }

        public void Initialize(int health, float speed,  RuntimeAnimatorController animatorController)
        {
            _animator.StopPlayback();
            _animator.runtimeAnimatorController = animatorController;
            _animator.speed = speed;
            
            _enemyHealthController.Initialize(health);
            _enemyHealthController.OnDied += Die;
            _speed = speed;
            _asyncMessageBus.Publish(new EnemySpawnEvent(this));
        }

        private void OnDisable()
        {
            _enemyHealthController.OnDied -= Die;
            OnEnemyDie = null;
        }

        private void FixedUpdate()
        {
            MoveTowardsPlayer();
        }

        private void MoveTowardsPlayer()
        {
            if (_player == null) return;
        
            var direction = (_player.transform.position - transform.position).normalized;
            _rigidbody2D.velocity = direction * _speed;

            if (direction.x == 0) return;
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
            transform.localScale = scale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Player")) return;
            var player = collision.collider.GetComponent<Player.Player>();
            if (player == null) return;
            player.TakeDamage();
        }

        private void Die()
        {
            OnEnemyDie?.Invoke();
            _asyncMessageBus.Publish(new EnemyDiedEvent(this));
        }
    }
}