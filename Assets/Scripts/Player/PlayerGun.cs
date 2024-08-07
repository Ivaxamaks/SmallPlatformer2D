using Common.Pool;
using Events;
using Sounds;
using UniTaskPubSub;
using UnityEngine;

namespace Player
{
    public class PlayerGun : MonoBehaviour
    {
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        
        [SerializeField] private Transform _firePoint;

        private MonoBehaviourPool<Bullet> _bulletPool;
        private GameSettings _gameSettings;
        private AsyncMessageBus _asyncMessageBus;

        private float _nextFireTime;
        private float _fireRate;
        private int _ammoCount;
        private SoundController _soundController;
        private Animator _animator;

        public void Initialize(AsyncMessageBus asyncMessageBus,
            SoundController soundController,
            Animator animator,
            Bullet bulletPrefab,
            GameSettings gameSettings)
        {
            _animator = animator;
            _soundController = soundController;
            _asyncMessageBus = asyncMessageBus;
            _gameSettings = gameSettings;
            _fireRate = gameSettings.PlayerFireRate;
            _bulletPool ??= new MonoBehaviourPool<Bullet>(bulletPrefab, bulletPrefab.transform.parent);
            ChangeAmmoAmount(gameSettings.PlayerStartAmmoAmount);
        }

        public void AddAmmo(int ammoAmount)
        {
            ChangeAmmoAmount(ammoAmount + _ammoCount);
        }

        private void ChangeAmmoAmount(int currentAmount)
        {
            _ammoCount = currentAmount;
            _asyncMessageBus.Publish(new PlayerAmmoAmountChangedEvent(_ammoCount));
        }

        public void FireSingleShot()
        {
            if (!(Time.time >= _nextFireTime)) return;
            FireBullet();
            _nextFireTime = Time.time + 1f / _fireRate;
        }

        public void FireContinuous()
        {
            if (!(Time.time >= _nextFireTime)) return;
            FireBullet();
            _nextFireTime = Time.time + 1f / _fireRate;
        }

        private void FireBullet()
        {
            var bullet = _bulletPool.Take();
            bullet.transform.position = _firePoint.position;
            var direction = gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            bullet.Initialize(_gameSettings.PlayerDamage, direction * _gameSettings.PlayerBulletSpeed);
            bullet.OnDispose += () => _bulletPool.Release(bullet);
            _soundController.PlaySound(SoundType.Shoot);
            ChangeAmmoAmount(_ammoCount - 1);
            _animator.SetTrigger(Shoot);
        }

        public void Dispose()
        {
            _bulletPool.ReleaseAll();
        }
    }
}