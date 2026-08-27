using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class DashState : EnemyState
    {
        [SerializeField] private DashStateConfig _config;
        [SerializeField] private RadialPatternConfig _radialPatternConfig;

        private float _delayTimer = 0f;

        private Vector3 _dashDirection;
        private Vector3 _startPosition;

        private int _currentShotCount = 0;

        private float _shotTimer = 0f;

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
                Shoot();
            }

            if (Vector3.Distance(_enemy.transform.position, _startPosition) >= _config.DashDistance)
            {
                OnStateFinished();
            }
        }

        private void Shoot()
        {
            RadialPattern radialPattern = new RadialPattern(_radialPatternConfig);
            radialPattern.Shoot(_enemy.transform.position);
        }
    }
}
