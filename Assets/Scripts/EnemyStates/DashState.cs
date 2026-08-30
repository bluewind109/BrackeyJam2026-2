using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;
using MoreMountains.Tools;
namespace EnemyStates
{
    public class DashState : EnemyState
    {
        [SerializeField] private List<DashStateConfig> _phaseConfigs = new List<DashStateConfig>();
        [SerializeField] private PredictionLine _predictionLinePrefab;

        private DashStateConfig _currentStateConfig;
        private PredictionLine _currentPredictionLine;

        private float _delayTimer = 0f;

        private Vector3 _dashDirection;
        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private int _currentShotCount = 0;

        private float _shotTimer = 0f;
        private float _dashTimer = 0f;

        public override void Initialize(Enemy enemy, Player player)
        {
            base.Initialize(enemy, player);
            SetPhaseConfig(0);
            _currentPredictionLine = Instantiate(_predictionLinePrefab, _enemy.transform.position, Quaternion.identity);
            _currentPredictionLine.transform.SetParent(_enemy.transform);
            _currentPredictionLine.gameObject.SetActive(false);
        }

        public override void Enter()
        {
            Debug.Log("Entering Dash State");
            _delayTimer = 0f;
            _dashDirection = Vector3.zero;
            _endPosition = _player.transform.position;

            // Create a prediction line to show where the enemy will dash
            ShowPredictionLine();

            _dashTimer = 0f;
            _shotTimer = _currentStateConfig.ShotInterval;
            _currentShotCount = 0;

            _enemy.EnemyDisplay.PlayPrepareAnimation();
        }

        private void ShowPredictionLine()
        {
            if (_predictionLinePrefab == null) return;
            _currentPredictionLine.gameObject.SetActive(true);
            _currentPredictionLine.transform.localPosition = Vector3.zero;
            _currentPredictionLine.transform.localRotation = Quaternion.identity;

            float dashDistance = _currentStateConfig.DashDistance;
            // scale the prediction line to match the dash distance
            Vector3 scale = _currentPredictionLine.transform.localScale;
            scale.x = 2f;
            scale.y = dashDistance;
            _currentPredictionLine.transform.localScale = scale;

            // position the prediction line to start from the enemy's position and extend in the direction of the dash
            Vector3 directionToPlayer = (_endPosition - _enemy.transform.position).normalized;
            _currentPredictionLine.transform.position = _enemy.transform.position + directionToPlayer * (dashDistance / 2f);
            // rotate the prediction line to face the player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
            _currentPredictionLine.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public override void Exit()
        {
            _currentPredictionLine?.gameObject.SetActive(false);
        }

        public override void UpdateState()
        {
            if (_currentPredictionLine != null)
            {
                _currentPredictionLine.GameUpdate(_delayTimer, _currentStateConfig.DelayDuration);
            }

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
            _currentPredictionLine?.gameObject.SetActive(false);
            _startPosition = _enemy.transform.position;
            _dashDirection = (_endPosition - _enemy.transform.position).normalized;

            MMGameEvent.Trigger(GameDefine.BossEvents.EventName, stringParameter: GameDefine.BossEvents.State_Dash_Start);
            _enemy.EnemyDisplay.PlayCastAnimation(GameDefine.BossAttackEvents.State_Dash);
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
