using EnemyStates;
using UnityEngine;

public class FirstBoss : Enemy
{
    [SerializeField] private EnemyState _idleState;
    [SerializeField] private EnemyState _circleShotState;
    [SerializeField] private EnemyState _dashState;

    private EnemyState _currentState;

    void Start()
    {
        _idleState.StateFinished += OnIdleStateFinished;
        _circleShotState.StateFinished += OnCircleShotStateFinished;
        _dashState.StateFinished += OnDashStateFinished;

        SetState(_idleState);
    }

    void OnDestroy()
    {
        _idleState.StateFinished -= OnIdleStateFinished;
        _circleShotState.StateFinished -= OnCircleShotStateFinished;
        _dashState.StateFinished -= OnDashStateFinished;
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
        SetState(_circleShotState);
    }

    private void OnCircleShotStateFinished()
    {
        SetState(_idleState);
    }

    private void OnDashStateFinished()
    {
        SetState(_idleState);
    }
}
