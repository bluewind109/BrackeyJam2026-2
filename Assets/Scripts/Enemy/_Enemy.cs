using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected BossStats _bossStats;

    private Health _health;
    private Hurtbox _hurtbox;
    private Hitbox _hitbox;
    private Player _player;

    void Awake()
    {
        _health = GetComponentInChildren<Health>();
        _hurtbox = GetComponentInChildren<Hurtbox>();
        _hitbox = GetComponentInChildren<Hitbox>();

        if (_health != null)
        {
            _health.Initialize(_bossStats.MaxHealth);
            _health.onHealthChanged += OnHealthChanged;
            _health.onDeath += OnDeath;
        }

        if (_hurtbox != null)
        {
            _hurtbox.Initialize(_health);
        }

        if (_hitbox != null)
        {
            _hitbox.Initialize("Player");
            _hitbox.OnHit += OnHitPlayer;
        }
    }

    void OnDestroy()
    {
        if (_health != null)
        {
            _health.onHealthChanged -= OnHealthChanged;
            _health.onDeath -= OnDeath;
        }

        if (_hitbox != null)
        {
            _hitbox.OnHit -= OnHitPlayer;
        }
    }

    public virtual void Initialize(Player player)
    {
        _player = player;
    }

    public abstract void GameUpdate();

    private void OnHitPlayer(Hurtbox playerHurtbox)
    {
        playerHurtbox.TakeDamage(1);
    }

    private void OnDeath()
    {

    }

    private void OnHealthChanged(int value)
    {

    }
}
