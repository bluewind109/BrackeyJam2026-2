using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class DashState : EnemyState
    {
        [SerializeField] private DashStateConfig config;

        private float _delayDuration;
        private float _delayTimer = 0f;

        private float _dashSpeed;
        private float _dashDistance;
        private Vector3 _dashDirection;
        private Vector3 _startPosition;

        private int _projectilesPerShot;
        private float _projectileSpeed;
        private int _currentShotCount = 0;

        private float _shotInterval;
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

            _dashSpeed = config.DashSpeed;
            _dashDistance = config.DashDistance;
            _delayDuration = config.DelayDuration;

            _projectilesPerShot = config.ProjectilesPerShot;
            _projectileSpeed = config.ProjectileSpeed;
            _shotInterval = config.ShotInterval;

            _shotTimer = _shotInterval;
            _currentShotCount = 0;
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
            UpdateDash();

            bool isDelayFinished = _delayTimer >= _delayDuration;
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
            _enemy.transform.position += _dashDirection * _dashSpeed * Time.deltaTime;

            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _shotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                Shoot();
            }

            if (Vector3.Distance(_enemy.transform.position, _startPosition) >= _dashDistance)
            {
                OnStateFinished();
            }
        }

        private void Shoot()
        {
            List<Projectile> projectiles = new List<Projectile>();
            for (int i = 0; i < _projectilesPerShot; i++)
            {
                var projectile = EnemyProjectileManager.Instance.SpawnProjectile(_enemy.transform.position, Vector3.zero, 5f);
                if (projectile != null)
                {
                    projectiles.Add(projectile);
                }
            }

            RadialPattern radialPattern = new RadialPattern();
            radialPattern.Shoot(projectiles, _enemy.transform.position, _projectileSpeed);
        }
    }
}
