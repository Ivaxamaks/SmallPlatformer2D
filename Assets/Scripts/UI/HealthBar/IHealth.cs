using System;
using UnityEngine;

namespace UI.HealthBar
{
    public interface IHealth
    {
        public bool IsInitialized { get; }

        public event Action<float, float> HealthChanged;
        public event Action OnDied;
    
        public bool IsDead { get; }
        public Transform HealthBarAnchor { get; }
        public float MaxHealth { get; }
        public float Health { get; }
    
    }
}