using UnityEngine;

public class FireBall : MonoBehaviour
{
    private bool _isInitialized = false;
    private float _speed = 10f;
    private int _damage = 10;
    private Vector3 _direction;

    private Hitbox _hitbox;


	void Awake()
    {
        _hitbox = GetComponentInChildren<Hitbox>();
        if (_hitbox != null)
        {
            _hitbox.Initialize("Enemy");
            _hitbox.onHit += OnHit;
        }
    }

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

    private void OnHit(Hurtbox hurtbox)
    {
        hurtbox.TakeDamage(_damage);
    }
}
