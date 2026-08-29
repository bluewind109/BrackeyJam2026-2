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
    [SerializeField] private EnemyState _meteorRainState;
    [SerializeField] private EnemyState _slamAttackState;
    [SerializeField] private EnemyState _dashState;
    [SerializeField] private EnemyState _recoverState;
    [SerializeField] private EnemyState _phaseTransitionState;

    [SerializeField] private List<EnemyPhaseData> _phaseDatas = new List<EnemyPhaseData>();

    private EnemyPhaseData _currentPhaseData;
    private List<StateData> _currentStateDatas = new List<StateData>();

    private const float HEALTH_THRESHOLD_PHASE_2 = 4f / 5f;
    private const float HEALTH_THRESHOLD_PHASE_3 = 2f / 5f;

    private EnemyState _currentState;
    private int _currentPhase = 1;

    void Start()
    {
        _spawnState.StateFinished += OnSpawnStateFinished;
        _idleState.StateFinished += OnIdleStateFinished;
        _chaseState.StateFinished += OnChaseStateFinished;
        _shootState.StateFinished += OnShootStateFinished;
        _meteorRainState.StateFinished += OnMeteorRainStateFinished;
        _slamAttackState.StateFinished += OnSlamAttackStateFinished;
        _dashState.StateFinished += OnDashStateFinished;
        _recoverState.StateFinished += OnRecoverStateFinished;
        _phaseTransitionState.StateFinished += OnPhaseTransitionStateFinished;
    }

    void OnDestroy()
    {
        _spawnState.StateFinished -= OnSpawnStateFinished;
        _idleState.StateFinished -= OnIdleStateFinished;
        _chaseState.StateFinished -= OnChaseStateFinished;
        _shootState.StateFinished -= OnShootStateFinished;
        _meteorRainState.StateFinished -= OnMeteorRainStateFinished;
        _slamAttackState.StateFinished -= OnSlamAttackStateFinished;
        _dashState.StateFinished -= OnDashStateFinished;
        _recoverState.StateFinished -= OnRecoverStateFinished;
        _phaseTransitionState.StateFinished -= OnPhaseTransitionStateFinished;
    }

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        ((SpawnState)_spawnState).Initialize(_spriteRenderer);
        ((IdleState)_idleState).Initialize(this, player);
        ((ChaseState)_chaseState).Initialize(this, player);
        ((DashState)_dashState).Initialize(this, player);
        ((ShootState)_shootState).Initialize(this, player);
        ((MeteorRainState)_meteorRainState).Initialize(this, player);
        ((SlamAttackState)_slamAttackState).Initialize(this, player);
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

    protected override void OnHealthChanged(int value)
    {
        base.OnHealthChanged(value);

        float healthPercentage = _health.HealthPercentage;

        if (healthPercentage <= HEALTH_THRESHOLD_PHASE_3 && _currentPhase < 3)
        {
            SetPhase(3);
        }
        else if (healthPercentage <= HEALTH_THRESHOLD_PHASE_2 && _currentPhase < 2)
        {
            SetPhase(2);
        }
    }

    private void SetPhase(int phase)
    {
        if (phase <= _currentPhase) return;

        SetState(_phaseTransitionState);
        _currentPhase = phase;

        int phaseIndex = _currentPhase - 1;

        ((DashState)_dashState).SetPhaseConfig(phaseIndex);
        ((ShootState)_shootState).SetPhaseConfig(phaseIndex);
        ((MeteorRainState)_meteorRainState).SetPhaseConfig(phaseIndex);
        ((SlamAttackState)_slamAttackState).SetPhaseConfig(phaseIndex);

        Debug.Log($"Boss Phase changed to: {_currentPhase}");
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
        var phaseData = GetCurrentPhaseData();
        if (phaseData == null) return;

        _currentPhaseData = new EnemyPhaseData(phaseData.StateData);
        _currentStateDatas = new List<StateData>(_currentPhaseData.StateData);
        int totalWeight = _currentPhaseData.GetTotalWeight();

        if (totalWeight <= 0)
        {
            SetState(_currentStateDatas[0].State);
            return;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        EnemyState selectedState = null;
        foreach (var stateData in _currentStateDatas)
        {
            if (stateData == null || stateData.State == null) continue;

            cumulativeWeight += Mathf.Max(0, stateData.BaseWeight);
            if (randomValue < cumulativeWeight)
            {
                selectedState = stateData.State;
                SetState(selectedState);
                break;
            }
        }

        if (selectedState == null)
        {
            Debug.LogWarning("No valid state selected. Defaulting to the last state in the list.");
            SetState(_currentStateDatas[_currentStateDatas.Count - 1].State);
        }

        UpdatePhaseDataWeights(selectedState);
    }

    private void UpdatePhaseDataWeights(EnemyState selectedState)
    {
        // Reset selected state data's weight to 1
        // Increase the weight of all other state data by 1
        foreach (var stateData in _currentStateDatas)
        {
            if (stateData == null) continue;

            if (stateData.State == selectedState)
            {
                stateData.BaseWeight = 1;
            }
            else
            {
                stateData.BaseWeight += 1;
            }
        }
    }

    private EnemyPhaseData GetCurrentPhaseData()
    {
        int phaseIndex = _currentPhase - 1;
        if (phaseIndex < 0)
        {
            Debug.LogWarning($"Invalid phase index {phaseIndex}. Returning null.");
            return null;
        }

        if (phaseIndex >= _phaseDatas.Count)
        {
            Debug.LogWarning($"Phase index {phaseIndex} exceeds available phase data. Returning last phase's data.");
            phaseIndex = _phaseDatas.Count - 1;
        }

        return _phaseDatas[phaseIndex];
    }

    private void OnShootStateFinished()
    {
        SetState(_recoverState);
    }
    private void OnMeteorRainStateFinished()
    {
        SetState(_recoverState);
    }

    private void OnSlamAttackStateFinished()
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

    private void OnPhaseTransitionStateFinished()
    {
        SetState(_recoverState);
    }
}
