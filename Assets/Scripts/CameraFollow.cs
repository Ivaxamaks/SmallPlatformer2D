using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform; 
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private Vector3 _offset;

    private void LateUpdate()
    {
        if (_playerTransform == null) return;
        
        var targetPosition = _playerTransform.position + _offset;
        
        var smoothedPosition = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
        
        transform.position = smoothedPosition;
    }
}