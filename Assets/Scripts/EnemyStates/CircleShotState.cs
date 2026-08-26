using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class CircleShotState : EnemyState
    {
        private int _numberOfShots = 8;
		private int _projectilesPerShot = 10;
		private int _currentShotCount = 0;

        private float _shotInterval = 0.5f;
        private float _shotTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Circle Shot State");
            _shotTimer = 0f;
			_currentShotCount = 0;
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
				// Angle adjusted after each shot to create a circular pattern

                Shoot(_currentShotCount * (360f / _numberOfShots));
                if (_currentShotCount >= _numberOfShots)
                {
                    OnStateFinished();
                }
            }
        }

		private void Shoot(float startAngle = 0f)
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
