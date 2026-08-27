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
    [SerializeField] private EnemyState _radialShootState;
    [SerializeField] private EnemyState _spiralShootState;
    [SerializeField] private EnemyState _dashState;
    [SerializeField] private EnemyState _recoverState;

    private List<EnemyState> _attackStates = new List<EnemyState>();

    private EnemyState _currentState;

    void Start()
    {
        _spawnState.StateFinished += OnSpawnStateFinished;
        _idleState.StateFinished += OnIdleStateFinished;
        _chaseState.StateFinished += OnChaseStateFinished;
        _radialShootState.StateFinished += OnRadialShootStateFinished;
        _spiralShootState.StateFinished += OnSpiralShootStateFinished;
        _dashState.StateFinished += OnDashStateFinished;
        _recoverState.StateFinished += OnRecoverStateFinished;

        _attackStates.Add(_radialShootState);
        // _attackStates.Add(_spiralShootState);
        _attackStates.Add(_dashState);
    }

    void OnDestroy()
    {
        _spawnState.StateFinished -= OnSpawnStateFinished;
        _idleState.StateFinished -= OnIdleStateFinished;
        _chaseState.StateFinished -= OnChaseStateFinished;
        _radialShootState.StateFinished -= OnRadialShootStateFinished;
        _spiralShootState.StateFinished -= OnSpiralShootStateFinished;
        _dashState.StateFinished -= OnDashStateFinished;
        _recoverState.StateFinished -= OnRecoverStateFinished;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        ((SpawnState)_spawnState).Initialize(_spriteRenderer);
        ((ChaseState)_chaseState).Initialize(this, player);
        ((DashState)_dashState).Initialize(this,player);
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

    private void OnRadialShootStateFinished()
    {
        SetState(_recoverState);
    }

    private void OnSpiralShootStateFinished()
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
