using System;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public event Action OnDispose;

        private const float TimeToDeactivate = 5f;
    
        private int _damage;
    
        private Vector2 _velocity;

        public void Initialize(int damage, Vector2 velocity)
        {
            _damage = damage;
            _velocity = velocity;
            Invoke(nameof(Deactivate), TimeToDeactivate);
        }

        private void OnDisable()
        {
            OnDispose = null;
        }
    
        private void Update()
        {
            transform.Translate(_velocity * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.GetComponent<Enemy.Enemy>();
            if (enemy != null)
            {
                enemy.HealthController.TakeDamage(_damage);
            }
            
            Deactivate();
        }

        private void Deactivate()
        {
            CancelInvoke(); 
            OnDispose?.Invoke();
        }
    }
}