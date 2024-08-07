using Events;
using Sounds;
using UniTaskPubSub;
using UnityEngine;
using VContainer;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerGun _playerGun;
        [SerializeField] private PlayerMovement _playerMovement;
    
        private InputHandler.InputHandler _inputHandler;
        private GameSettings _gameSettings;
        private Bullet _bulletPrefab;
        private AsyncMessageBus _asyncMessageBus;
        private SoundController _soundController;

        [Inject]
        public void Construct(InputHandler.InputHandler inputHandler,
            GameSettings gameSettings,
            Bullet bulletPrefab,
            AsyncMessageBus asyncMessageBus,
            SoundController soundController)
        {
            _soundController = soundController;
            _inputHandler = inputHandler;
            _gameSettings = gameSettings;
            _bulletPrefab = bulletPrefab;
            _asyncMessageBus = asyncMessageBus;
        }
    
        public void Initialize()
        {
            _inputHandler.OnMove += HandleMove;
            _inputHandler.OnFire += HandleFire;
        
            _playerGun.Initialize(_asyncMessageBus, _soundController, _animator, _bulletPrefab, _gameSettings);
            _playerMovement.Initialize(_gameSettings.PlayerMoveSpeed, _animator);
        }

        public void Dispose()
        {
            _inputHandler.OnMove -= HandleMove;
            _inputHandler.OnFire -= HandleFire;
            _playerGun.Dispose();
        }

        public void TakeDamage()
        {
            _asyncMessageBus.Publish(new PlayerDiedEvent());
        }

        private void HandleMove(float direction)
        {
            _playerMovement.Move(direction);
        }

        private void HandleFire(bool isContinuous)
        {
            if (isContinuous)
            {
                _playerGun.FireContinuous();
            }
            else
            {
                _playerGun.FireSingleShot();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("AmmoPickup")) return;
            var ammoPickup = other.GetComponent<AmmoPickup>();
            _playerGun.AddAmmo(ammoPickup.AmmoAmount);
            ammoPickup.Dispose();
            _soundController.PlaySound(SoundType.Pickup);
        }
    }
}