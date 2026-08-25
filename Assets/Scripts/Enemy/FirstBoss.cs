using EnemyStates;
using UnityEngine;

public class FirstBoss : Enemy
{
    [SerializeField] private EnemyState _idleState;
    [SerializeField] private EnemyState _circleShotState;

    private EnemyState _currentState;

    void Start()
    {
        _idleState.StateFinished += OnIdleStateFinished;
        _circleShotState.StateFinished += OnCircleShotStateFinished;

        SetState(_idleState);
    }

    void OnDestroy()
    {
        _idleState.StateFinished -= OnIdleStateFinished;
        _circleShotState.StateFinished -= OnCircleShotStateFinished;
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
}
