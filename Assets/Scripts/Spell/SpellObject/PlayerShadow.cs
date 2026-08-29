using System;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    enum ShadowState
    {
        Idle,
        Moving
    }

    [SerializeField] private float _waitDuration = 1f;
    private float _waitTimer = 0f;
    private float _speed = 10f;
    private int _damage = 1;

    private Player _player;
    private Hitbox _hitbox;

    private ShadowState _currentState = ShadowState.Idle;
    private bool _isInitialized = false;

    void Awake()
    {
        _hitbox = GetComponentInChildren<Hitbox>();
        if (_hitbox != null)
        {
            _hitbox.Initialize("Enemy");
            _hitbox.OnHit += OnHit;
        }
    }

    void OnDestroy()
    {
        if (_hitbox != null)
        {
            _hitbox.OnHit -= OnHit;
        }
    }

    public void Initialize(Player player, float speed, int damage)
    {
        _player = player;
        _waitTimer = 0f;
        _speed = speed;
        _damage = damage;
        _isInitialized = true;
    }

    public void GameUpdate()
    {
        if (!_isInitialized) return;

        switch (_currentState)
        {
            case ShadowState.Idle:
                HandleIdleState();
                break;
            case ShadowState.Moving:
                HandleMovingState();
                break;
        }
    }

    private void SetState(ShadowState newState)
    {
        if (newState == _currentState) return;
        _currentState = newState;

        switch (_currentState)
        {
            case ShadowState.Idle:
                _waitTimer = 0f;
                break;
            case ShadowState.Moving:
                break;
        }
    }

    private void HandleIdleState()
    {
        if (_waitTimer >= _waitDuration) return;

        _waitTimer += Time.deltaTime;
        if (_waitTimer >= _waitDuration)
        {
            // this.gameObject.SetActive(false);
            // Destroy(this.gameObject, 0.1f);
            SetState(ShadowState.Moving);
        }
    }

    private void HandleMovingState()
    {
        if (_player == null) return;

        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        // Check if the shadow has reached the player
        if (Vector3.Distance(transform.position, _player.transform.position) < 0.1f)
        {
            _player = null;
            Release();
        }
    }

    private void Release()
    {
        PlayerShadowManager.Instance.UnregisterPlayerShadow(this);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    private void OnHit(Hurtbox hurtbox)
    {
        hurtbox.TakeDamage(_damage);
    }
}
