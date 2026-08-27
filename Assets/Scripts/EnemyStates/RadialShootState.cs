using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class RadialShootState : EnemyState
    {
        [SerializeField] private RadialShootStateConfig _config;
        [SerializeField] private RadialPatternConfig _shootPatternConfig;

		private int _currentShotCount = 0;
        private float _shotTimer = 0f;
        private RadialPattern _radialPattern;

        public override void Enter()
        {
            Debug.Log("Entering Radial Shoot State");
            _shotTimer = 0f;
			_currentShotCount = 0;
			_radialPattern = new RadialPattern();
        }

        public override void Exit()
        {
            // Logic for exiting the Radial Shoot state
        }

        public override void UpdateState()
        {
            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _config.ShotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                // Shoot(_currentShotCount * (360f / _numberOfShots));
                Shoot();
                if (_currentShotCount >= _config.NumberOfShots)
                {
                    OnStateFinished();
                }
            }
        }

		private void Shoot()
		{
			List<Projectile> projectiles = new List<Projectile>();
			for (int i = 0; i < _config.ProjectilesPerShot; i++)
			{
				var projectile = EnemyProjectileManager.Instance.SpawnProjectile(transform.position, Vector3.zero, 5f);
				if (projectile != null)
				{
					projectiles.Add(projectile);
				}
			}

            _radialPattern.Shoot(projectiles, transform.position, _shootPatternConfig.ProjectileSpeed);
		}
    }
}
