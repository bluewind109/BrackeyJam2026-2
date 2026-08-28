using System;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;

public class FirstBoss : Enemy
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private EnemyState _spawnState;
    [SerializeField] private EnemyState _idleState;
    [SerializeField] private EnemyState _chaseState;
    [SerializeField] private EnemyState _shootState;
    [SerializeField] private EnemyState _slamState;
    [SerializeField] private EnemyState _dashState;
    [SerializeField] private EnemyState _recoverState;

    [SerializeField] private List<EnemyState> _attackStates = new List<EnemyState>();

    private EnemyState _currentState;

    void Start()
    {
        _spawnState.StateFinished += OnSpawnStateFinished;
        _idleState.StateFinished += OnIdleStateFinished;
        _chaseState.StateFinished += OnChaseStateFinished;
        _shootState.StateFinished += OnShootStateFinished;
        _slamState.StateFinished += OnSlamStateFinished;
        _dashState.StateFinished += OnDashStateFinished;
        _recoverState.StateFinished += OnRecoverStateFinished;
    }

    void OnDestroy()
    {
        _spawnState.StateFinished -= OnSpawnStateFinished;
        _idleState.StateFinished -= OnIdleStateFinished;
        _chaseState.StateFinished -= OnChaseStateFinished;
        _shootState.StateFinished -= OnShootStateFinished;
        _slamState.StateFinished -= OnSlamStateFinished;
        _dashState.StateFinished -= OnDashStateFinished;
        _recoverState.StateFinished -= OnRecoverStateFinished;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        ((SpawnState)_spawnState).Initialize(_spriteRenderer);
        ((ChaseState)_chaseState).Initialize(this, player);
        ((DashState)_dashState).Initialize(this, player);
        ((ShootState)_shootState).Initialize(player);
        ((SlamAttackState)_slamState).Initialize(player, 8);
        SetState(_spawnState);
    }

    private void SetState(EnemyState newState)
    {
        if (newState == null) return;
        if (_currentState == newState) return;

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public override void GameUpdate()
    {
        _currentState?.UpdateState();
    }

    private void OnSpawnStateFinished()
    {
        SetState(_idleState);
    }

    private void OnIdleStateFinished()
    {
        SetState(_chaseState);
    }

    private void OnChaseStateFinished()
    {
        SetRandomAttackState();
    }

    private void SetRandomAttackState()
    {
        if (_attackStates.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, _attackStates.Count);
        SetState(_attackStates[randomIndex]);
    }

    private void OnShootStateFinished()
    {
        SetState(_recoverState);
    }
    private void OnSlamStateFinished()
    {
        SetState(_recoverState);
    }

    private void OnDashStateFinished()
    {
        SetState(_recoverState);
    }
    private void OnRecoverStateFinished()
    {
        SetState(_chaseState);
    }
}
