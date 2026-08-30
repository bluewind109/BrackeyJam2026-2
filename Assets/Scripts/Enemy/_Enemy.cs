using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected BossStats _bossStats;
    [SerializeField] protected EnemyDisplay _enemyDisplay;

    public EnemyDisplay EnemyDisplay => _enemyDisplay;


    protected Health _health;
    private Hurtbox _hurtbox;
    private Hitbox _hitbox;
    private Player _player;

    protected int _currentPhase = 1;
    public int CurrentPhase => _currentPhase;

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

        _enemyDisplay.Initialize(this);
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
        ClampInsideScreen();
    }

    public abstract void GameUpdate();

    public void Move(Vector3 movement)
    {
        SetPosition(transform.position + movement);
    }

    public void SetPosition(Vector3 targetPosition)
    {
        transform.position = ScreenBoundsUtility.ClampPositionInsideCamera(Camera.main, transform, targetPosition);
    }

    public void ClampInsideScreen()
    {
        SetPosition(transform.position);
    }

    private void OnHitPlayer(Hurtbox playerHurtbox)
    {
        playerHurtbox.TakeDamage(1);
    }

    protected virtual void OnDeath()
    {
    }

    protected virtual void OnHealthChanged(int currentHealth)
    {
        Debug.Log($"{gameObject.name} health changed to {currentHealth}");
    }
}
