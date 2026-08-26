using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	public event Action<EnemyProjectile> OnReleased;

	private bool _isInitialized = false;
	private bool _isActive = false;
	public bool IsActive => _isActive;

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
		this.gameObject.SetActive(true);
		_isInitialized = true;
	}

	public void GameUpdate()
	{
        transform.position += _direction * _speed * Time.deltaTime;
	}

	private void OnHit(Hurtbox hurtbox)
	{
		hurtbox.TakeDamage(_damage);
	}

	private void Release()
	{
		_isActive = false;
		gameObject.SetActive(false);
		OnReleased?.Invoke(this);
	}
}
