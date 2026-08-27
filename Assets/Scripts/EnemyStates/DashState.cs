using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class DashState : EnemyState
    {
        [SerializeField] private DashStateConfig _config;
        [SerializeField] private RadialPatternConfig _radialPatternConfig;
        [SerializeField] private SidewayPatternConfig _sidewayPatternConfig;

        private float _delayTimer = 0f;

        private Vector3 _dashDirection;
        private Vector3 _startPosition;

        private int _currentShotCount = 0;

        private float _shotTimer = 0f;

        private float _dashDuration = 2f;
        private float _dashTimer = 0f;

        private Enemy _enemy;
        private Player _player;

        public void Initialize(Enemy enemy, Player player)
        {
            _enemy = enemy;
            _player = player;
        }

        public override void Enter()
        {
            Debug.Log("Entering Dash State");
            _delayTimer = 0f;
            _dashDirection = Vector3.zero;

            _shotTimer = _config.ShotInterval;
            _currentShotCount = 0;
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
            UpdateDash();

            bool isDelayFinished = _delayTimer >= _config.DelayDuration;
            if (!isDelayFinished) _delayTimer += Time.deltaTime;
            if (isDelayFinished && _dashDirection == Vector3.zero)
            {
                StartDash();
            }

            if (isDelayFinished && _dashDirection != Vector3.zero)
            {
                _dashTimer += Time.deltaTime;
                if (_dashTimer >= _dashDuration)
                {
                    OnDashFinished();
                }
            }
        }

        private void StartDash()
        {
            _startPosition = _enemy.transform.position;
            _dashDirection = (_player.transform.position - _enemy.transform.position).normalized;
        }

        private void UpdateDash()
        {
            if (_dashDirection == Vector3.zero) return;
            _enemy.Move(_dashDirection * _config.DashSpeed * Time.deltaTime);

            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _config.ShotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                ShootSideway();
            }

            float traveledDistance = Vector3.Distance(_enemy.transform.position, _startPosition);
            bool hasReachedDashDistance = traveledDistance >= _config.DashDistance;
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
            SidewayPattern sidewayPattern = new SidewayPattern(_sidewayPatternConfig);
            sidewayPattern.Shoot(_enemy.transform.position, _dashDirection);
        }

        private void ShootRadial()
        {
            RadialPattern radialPattern = new RadialPattern(_radialPatternConfig);
            radialPattern.Shoot(_enemy.transform.position);
        }
    }
}
