using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected BossStats _bossStats;

    private Health _health;
    private Hurtbox _hurtbox;
    private Player _player;

    void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _hurtbox = GetComponentInChildren<Hurtbox>();

        _health.Initialize(_bossStats.MaxHealth);

        if (_health != null)
        {
            _health.onHealthChanged += OnHealthChanged;
            _health.onDeath += OnDeath;
        }

        if (_hurtbox != null)
        {
            _hurtbox.OnHit += TakeDamage;
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
            _hurtbox.OnHit -= TakeDamage;
        }
    }

    public virtual void Initialize(Player player)
    {
        _player = player;
    }

    public abstract void GameUpdate();

	protected void TakeDamage(int damageTaken)
    {
        _health?.TakeDamage(damageTaken);
    }

	private void OnDeath()
	{
	}

	private void OnHealthChanged(int value)
	{
	}
}
