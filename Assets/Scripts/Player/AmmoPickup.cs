using System;
using UnityEngine;

namespace Player
{
    public class AmmoPickup : MonoBehaviour
    {
        public Action OnDisposed;
    
        public int AmmoAmount { get; private set; }

        public void Initialize(int ammoAmount)
        {
            AmmoAmount = ammoAmount;
        }

        private void OnDisable()
        {
            OnDisposed = null;
        }

        public void Dispose()
        {
            OnDisposed?.Invoke();
        }
    }
}