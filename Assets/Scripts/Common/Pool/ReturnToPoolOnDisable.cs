using System;
using UnityEngine;

namespace Common.Pool
{
    public class ReturnToPoolOnDisable : MonoBehaviour
    {
        private Action<GameObject> _returnToPool;

        public void Initialize(Action<GameObject> returnToPool)
        {
            _returnToPool = returnToPool;
        }

        public void OnDisable()
        {
            _returnToPool?.Invoke(gameObject);
        }
    }
}