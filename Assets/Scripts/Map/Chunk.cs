using System;
using UnityEngine;

namespace Map
{
    public class Chunk : MonoBehaviour
    {
        public event Action<Chunk> OnPlayerEnter;
        public event Action<Chunk> OnPlayerStay;
        public bool IsActive { get; private set; }

        public void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
            IsActive = true;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke(this);
            }
        }
        
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                OnPlayerStay?.Invoke(this);
            }
        }

        private void OnDisable()
        {
            OnPlayerEnter = null;
        }
    }
}