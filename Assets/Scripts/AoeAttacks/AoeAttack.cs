using System;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttack : MonoBehaviour
{
    public event Action<AoeAttack> OnAoeAttackFinished;

    public enum AoeState
    {
        Inactive,
        Delaying,
        Executing,
        Persisting
    }
    private AoeState _currentState = AoeState.Inactive;

    [SerializeField] private SpriteRenderer predictionZone;

    private Hitbox _hitbox;
    private Collider2D _hitboxCollider;
    private readonly Collider2D[] _overlapResults = new Collider2D[16];
    private readonly HashSet<Hurtbox> _uniqueHurtboxes = new HashSet<Hurtbox>();

    private int _damage = 1;

    private float _delayDuration = 1f;
    private float _delayTimer = 0f;

    private float _persistenceDuration = 0.1f;
    private float _persistenceTimer = 0f;


    void Awake()
    {
        _hitbox = GetComponentInChildren<Hitbox>();

        if (_hitbox == null)
        {
            Debug.LogError("AoeAttack requires a child Hitbox component.");
            return;
        }

        _hitboxCollider = _hitbox.GetComponent<Collider2D>();
        if (_hitboxCollider == null)
        {
            Debug.LogError("AoeAttack Hitbox requires a Collider2D component.");
            return;
        }

        _hitbox.Initialize("Player");
    }

    public virtual void Initialize(float delayDuration, int damage, float spawnRadius)
    {
        if (_hitbox == null)
        {
            Debug.LogError("Cannot initialize AoeAttack without Hitbox.");
            return;
        }

        gameObject.SetActive(true);
        _delayDuration = delayDuration;
        _damage = damage;
        this.transform.localScale = new Vector3(spawnRadius, spawnRadius, 1f);
        _delayTimer = 0f;
        _persistenceTimer = 0f;
        predictionZone.transform.localScale = Vector3.zero;
        _hitbox.OnHit += OnHit;
        _currentState = AoeState.Delaying;
    }

    private void OnHit(Hurtbox hurtbox)
    {
        if (_currentState != AoeState.Persisting) return;
        hurtbox.TakeDamage(_damage);
    }

    private void Release()
    {
        gameObject.SetActive(false);
        _hitbox.OnHit -= OnHit;
        _currentState = AoeState.Inactive;
        OnAoeAttackFinished?.Invoke(this);
    }

    public void GameUpdate()
    {
        switch (_currentState)
        {
            case AoeState.Delaying:
                UpdateDelay(Time.deltaTime);
                break;
            case AoeState.Executing:
                // Visualize the attack
                UpdateExecuting(Time.deltaTime);
                break;
            case AoeState.Persisting:
                UpdatePersisting(Time.deltaTime);
                break;
        }
    }

    private void UpdateDelay(float deltaTime)
    {
        _delayTimer += deltaTime;
        float scale = Mathf.Clamp01(_delayTimer / _delayDuration);
        predictionZone.transform.localScale = new Vector3(scale, scale, 1f);

        bool isDelayComplete = _delayTimer >= _delayDuration;
        if (isDelayComplete)
        {
            ExecuteAoeAttack();
        }
    }

    protected virtual void UpdateExecuting(float deltaTime)
    {

    }

    protected virtual void UpdatePersisting(float deltaTime)
    {
        _persistenceTimer += deltaTime;
        if (_persistenceTimer >= _persistenceDuration)
        {
            Release();
        }
    }

    protected virtual void ExecuteAoeAttack()
    {
        _currentState = AoeState.Executing;
    }

    protected virtual void StartPersisting()
    {
        DealDamageToPlayersAlreadyInside();
        _currentState = AoeState.Persisting;
    }

    private void DealDamageToPlayersAlreadyInside()
    {
        if (_hitboxCollider == null) return;

        int overlapCount = _hitboxCollider.Overlap(ContactFilter2D.noFilter, _overlapResults);
        if (overlapCount <= 0) return;

        _uniqueHurtboxes.Clear();
        for (int i = 0; i < overlapCount; i++)
        {
            Collider2D overlap = _overlapResults[i];
            if (overlap == null || overlap.transform.parent == null) continue;
            if (!overlap.transform.parent.CompareTag("Player")) continue;

            Hurtbox hurtbox = overlap.GetComponent<Hurtbox>();
            if (hurtbox == null || !_uniqueHurtboxes.Add(hurtbox)) continue;

            hurtbox.TakeDamage(_damage);
        }
    }
}
