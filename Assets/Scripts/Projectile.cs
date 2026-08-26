using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	public event Action<Projectile> OnReleased;
	
	protected Hitbox _hitbox;
	private float _speed = 10f;
	private int _damage = 10;
	private Vector3 _direction;

	private bool _isActive = false;
	public bool IsActive => _isActive;

	void Awake()
	{
		_hitbox = GetComponentInChildren<Hitbox>();

		if (_hitbox != null)
		{
			_hitbox.onHit += OnHit;
		}
	}

	public virtual void Initialize(Vector3 direction, float speed, string targetTag)
	{
		_direction = direction;
		_speed = speed;
		gameObject.SetActive(true);
		_isActive = true;
		if (_hitbox != null)
		{
			_hitbox.Initialize(targetTag);
		}
	}

	public void GameUpdate()
	{
		if (!_isActive) return;

		transform.position += _direction * _speed * Time.deltaTime;
	}

	protected virtual void OnHit(Hurtbox hurtbox)
	{
		hurtbox.TakeDamage(_damage);
	}

	protected virtual void Release()
	{
		_isActive = false;
		gameObject.SetActive(false);
		OnReleased?.Invoke(this);
	}
}
