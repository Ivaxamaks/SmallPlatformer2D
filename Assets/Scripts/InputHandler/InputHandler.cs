using System;
using UnityEngine;
using VContainer.Unity;

namespace InputHandler
{
    public class InputHandler : ITickable, IFixedTickable
    {
        public event Action<float> OnMove;
        public event Action<bool> OnFire;

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnFire?.Invoke(false);
            }
            else if (Input.GetMouseButton(0))
            {
                OnFire?.Invoke(true);
            }
        }

        public void FixedTick()
        {
            float direction = 0;
            if (Input.GetKey(KeyCode.A))
            {
                direction = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction = 1;
            }

            OnMove?.Invoke(direction);
        }
    }
}