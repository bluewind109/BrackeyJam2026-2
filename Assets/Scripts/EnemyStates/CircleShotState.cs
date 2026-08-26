using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class CircleShotState : EnemyState
    {
        [SerializeField] private CircleShotStateConfig config;

        private int _numberOfShots;
		private int _projectilesPerShot;
		private int _currentShotCount = 0;

        private float _shotInterval;
        private float _shotTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Circle Shot State");
            _shotTimer = 0f;
			_currentShotCount = 0;
            _numberOfShots = config.NumberOfShots;
            _projectilesPerShot = config.ProjectilesPerShot;
            _shotInterval = config.ShotInterval;
        }

        public override void Exit()
        {
            // Logic for exiting the Circle Shot state
        }

        public override void UpdateState()
        {
            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _shotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                // Shoot(_currentShotCount * (360f / _numberOfShots));
                Shoot();
                if (_currentShotCount >= _numberOfShots)
                {
                    OnStateFinished();
                }
            }
        }

		private void Shoot()
		{
			List<Projectile> projectiles = new List<Projectile>();
			for (int i = 0; i < _projectilesPerShot; i++)
			{
				var projectile = EnemyProjectileManager.Instance.SpawnProjectile(transform.position, Vector3.zero, 5f);
				if (projectile != null)
				{
					projectiles.Add(projectile);
				}
			}

			float projectileSpeed = 2.5f;
			RadialPattern radialPattern = new RadialPattern();
			radialPattern.Shoot(projectiles, transform.position, projectileSpeed);
		}
    }
}
