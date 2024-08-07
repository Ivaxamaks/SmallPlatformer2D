using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private float _moveSpeed;
        private Animator _animator;

        public void Initialize(float moveSpeed, Animator animator)
        {
            _animator = animator;
            _moveSpeed = moveSpeed;
        }
    
        public void Move(float direction)
        {
            var velocity = new Vector2(direction * _moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = velocity;
            _animator.SetBool(IsRun, direction != 0);

            if (direction == 0) return;
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
            transform.localScale = scale;
        }
    }
}