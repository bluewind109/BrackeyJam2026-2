using System;
using System.Collections.Generic;
using EnemyStates;
using UnityEngine;

public class FirstBoss : Enemy
{
    [SerializeField] private EnemyState _idleState;
    [SerializeField] private EnemyState _chaseState;
    [SerializeField] private EnemyState _circleShotState;
    [SerializeField] private EnemyState _dashState;

    private List<EnemyState> _attackStates = new List<EnemyState>();

    private EnemyState _currentState;

    void Start()
    {
        _idleState.StateFinished += OnIdleStateFinished;
        _chaseState.StateFinished += OnChaseStateFinished;
        _circleShotState.StateFinished += OnCircleShotStateFinished;
        _dashState.StateFinished += OnDashStateFinished;

        _attackStates.Add(_circleShotState);
        _attackStates.Add(_dashState);
    }

    void OnDestroy()
    {
        _idleState.StateFinished -= OnIdleStateFinished;
        _chaseState.StateFinished -= OnChaseStateFinished;
        _circleShotState.StateFinished -= OnCircleShotStateFinished;
        _dashState.StateFinished -= OnDashStateFinished;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        ((ChaseState)_chaseState).Initialize(this, player);
        ((DashState)_dashState).Initialize(this,player);
        SetState(_idleState);
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

    private void OnCircleShotStateFinished()
    {
        SetState(_chaseState);
    }

    private void OnDashStateFinished()
    {
        SetState(_chaseState);
    }
}
