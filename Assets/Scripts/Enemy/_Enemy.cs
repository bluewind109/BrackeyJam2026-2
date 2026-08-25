using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
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
            _hurtbox.onHit += TakeDamage;
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
            _hurtbox.onHit -= TakeDamage;
        }
    }

    public abstract void GameUpdate();

	private void TakeDamage(int damageTaken)
	{
	}

	private void OnDeath()
	{
	}

	private void OnHealthChanged(int value)
	{
	}
}
