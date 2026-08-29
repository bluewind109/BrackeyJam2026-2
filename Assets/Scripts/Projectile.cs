using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	public event Action<Projectile> OnReleased;

	private ProjectileBehavior _behavior;
	protected Hitbox _hitbox;
	private float _speed = 10f;
	private int _damage = 10;
	protected Vector3 _direction;

	private bool _isActive = false;
	public bool IsActive => _isActive;

	void Awake()
	{
		_hitbox = GetComponentInChildren<Hitbox>();

		if (_hitbox != null)
		{
			_hitbox.OnHit += OnHit;
		}
	}

	public virtual void Initialize(Vector3 direction, float speed, int damage, string targetTag)
	{
		_direction = direction;
		_speed = speed;
		_damage = damage;
		gameObject.SetActive(true);
		_isActive = true;
		if (_hitbox != null)
		{
			_hitbox.Initialize(targetTag);
		}
	}

	public virtual void GameUpdate()
	{
		if (!_isActive) return;

		transform.position += _direction * _speed * Time.deltaTime;
		// _behavior?.GameUpdate();

		// if out of bounds, release the projectile
		if (IsOutOfBounds())
		{
			Release();
		}
	}

	private bool IsOutOfBounds()
	{
		return ScreenBoundsUtility.IsOutsideCamera(Camera.main, transform);
	}

	protected virtual void OnHit(Hurtbox hurtbox)
	{
		hurtbox.TakeDamage(_damage);
		Release();
	}

	protected virtual void Release()
	{
		_isActive = false;
		gameObject.SetActive(false);
		OnReleased?.Invoke(this);
	}
}
