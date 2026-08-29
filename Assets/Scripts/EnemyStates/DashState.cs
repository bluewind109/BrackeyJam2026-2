using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class DashState : EnemyState
    {
        [SerializeField] private List<DashStateConfig> _phaseConfigs = new List<DashStateConfig>();
        private DashStateConfig _currentStateConfig;

        private float _delayTimer = 0f;

        private Vector3 _dashDirection;
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private int _currentShotCount = 0;

        private float _shotTimer = 0f;
        private float _dashTimer = 0f;

        private Enemy _enemy;
        private Player _player;

        public void Initialize(Enemy enemy, Player player)
        {
            _enemy = enemy;
            _player = player;
           SetPhaseConfig(0);
        }

        public override void Enter()
        {
            Debug.Log("Entering Dash State");
            _delayTimer = 0f;
            _dashDirection = Vector3.zero;
            _endPosition = _player.transform.position;

            _dashTimer = 0f;
            _shotTimer = _currentStateConfig.ShotInterval;
            _currentShotCount = 0;
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
            UpdateDash();

            bool isDelayFinished = _delayTimer >= _currentStateConfig.DelayDuration;
            if (!isDelayFinished) _delayTimer += Time.deltaTime;
            if (isDelayFinished && _dashDirection == Vector3.zero)
            {
                StartDash();
            }

            if (isDelayFinished && _dashDirection != Vector3.zero)
            {
                _dashTimer += Time.deltaTime;
                if (_dashTimer >= _currentStateConfig.DashDuration)
                {
                    OnDashFinished();
                }
            }
        }

        private void StartDash()
        {
            _startPosition = _enemy.transform.position;
            _dashDirection = (_endPosition - _enemy.transform.position).normalized;
        }

        private void UpdateDash()
        {
            if (_dashDirection == Vector3.zero) return;
            _enemy.Move(_dashDirection * _currentStateConfig.DashSpeed * Time.deltaTime);

            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _currentStateConfig.ShotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                ShootSideway();
            }

            float traveledDistance = Vector3.Distance(_enemy.transform.position, _startPosition);
            bool hasReachedDashDistance = traveledDistance >= _currentStateConfig.DashDistance;
            if (hasReachedDashDistance)
            {
                OnDashFinished();
            }
        }

        private void OnDashFinished()
        {
            ShootRadial();
            OnStateFinished();
        }

        private void ShootSideway()
        {
            SidewayPattern sidewayPattern = new SidewayPattern(_currentStateConfig.SidewayPatternConfig);
            sidewayPattern.Shoot(_enemy.transform.position, _dashDirection);
        }

        private void ShootRadial()
        {
            RadialPattern radialPattern = new RadialPattern(_currentStateConfig.RadialPatternConfig);
            radialPattern.Shoot(_enemy.transform.position);
        }

        public void SetPhaseConfig(int phaseIndex)
        {
            if (phaseIndex < 0 || _phaseConfigs.Count == 0)
            {
                Debug.LogWarning($"Invalid phase index {phaseIndex}. Using default config.");
                return;
            }

            if (phaseIndex >= _phaseConfigs.Count)
            {
                Debug.LogWarning($"Phase index {phaseIndex} exceeds available configs. Using last config.");
                phaseIndex = _phaseConfigs.Count - 1;
            }

            _currentStateConfig = _phaseConfigs[phaseIndex];
        }
    }
}
