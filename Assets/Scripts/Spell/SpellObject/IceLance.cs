using UnityEngine;

public class IceLance : MonoBehaviour
{
    private bool _isInitialized = false;
    private float _speed = 10f;
    private Vector3 _direction;

    public void Initialize(Vector3 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
        _isInitialized = true;
    }

    void Update()
    {
        if (!_isInitialized) return;

        transform.position += _direction * _speed * Time.deltaTime;
    }
}
