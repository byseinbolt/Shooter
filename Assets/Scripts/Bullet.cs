using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    
    private Vector3 _targetPosition;
    
    public void Initialize(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        var distanceBefore = Vector3.Distance(transform.position, _targetPosition);
        
        var moveDirection = (_targetPosition - transform.position).normalized;
        transform.position += moveDirection * (_speed * Time.deltaTime);

        var distanceAfter = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBefore<distanceAfter)
        {
            Destroy(gameObject);
        }
    }
}