using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	public event Action<Projectile> OnReleased;

	private ProjectileBehavior _behavior;
	protected Hitbox _hitbox;
	private float _speed = 10f;
	private int _damage = 10;
	private Vector3 _direction;

	private bool _isActive = false;
	public bool IsActive => _isActive;

	private float horizontalBoundary = 20f;
	private float verticalBoundary = 15f;

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

		_behavior?.GameUpdate();

		// if out of bounds, release the projectile
		if (IsOutOfBounds())
		{
			Release();
		}
	}

	private bool IsOutOfBounds()
	{
		return Mathf.Abs(transform.position.x) > horizontalBoundary || 
			   Mathf.Abs(transform.position.y) > verticalBoundary;
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
