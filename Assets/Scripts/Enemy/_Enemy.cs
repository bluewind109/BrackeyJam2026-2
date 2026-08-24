using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Health _health;
    private Hurtbox _hurtbox;

    void Awake()
    {
        _health = GetComponent<Health>();
        _hurtbox = GetComponent<Hurtbox>();

        if (_health != null)
        {
            _health.onHealthChanged += OnHealthChanged;
            _health.onDeath += OnDeath;
        }

        if (_hurtbox != null)
        {
            _hurtbox.onHit += OnHit;
        }
    }

    void OnDestroy()
    {
        if (_health != null)
        {
            _health.onHealthChanged -= OnHealthChanged;
            _health.onDeath -= OnDeath;
        }

        if (_hurtbox != null)
        {
            _hurtbox.onHit -= OnHit;
        }
    }

	private void OnHit(int damageTaken)
	{
		throw new NotImplementedException();
	}

	private void OnDeath()
	{
		throw new NotImplementedException();
	}

	private void OnHealthChanged(int value)
	{
		throw new NotImplementedException();
	}
}
