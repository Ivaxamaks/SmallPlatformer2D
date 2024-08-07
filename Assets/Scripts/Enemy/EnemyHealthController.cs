using System;
using UI.HealthBar;
using UnityEngine;

namespace Enemy
{
    public class EnemyHealthController : MonoBehaviour, IHealth
    {
        public event Action<float, float> HealthChanged;
        public event Action OnDied;
    
        public bool IsInitialized { get; private set; }
        public bool IsDead => Health <= 0;
    
        [field: SerializeField]
        public Transform HealthBarAnchor { get; private set; }
    
        public float MaxHealth { get; private set; }
        public float Health { get; private set; }

        public void Initialize(int health)
        {
            MaxHealth = health;
            Health = health;

            IsInitialized = true;
            HealthChanged?.Invoke(health, MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            if(IsDead) return;

            if (damage >= Health)
            {
                Health = 0;
            }
            else
            {
                Health -= damage;
            }
        
            HealthChanged?.Invoke(Health, MaxHealth);
            if(!IsDead) return;
            OnDied?.Invoke();
        }
    }
}