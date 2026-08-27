using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
	public class SpiralShootState : EnemyState
	{
        [SerializeField] private SpiralPatternConfig _shootPatternConfig;

		private int _currentShotCount = 0;
        private float _shotTimer = 0f;
        private SpiralPattern _spiralPattern;

		public override void Enter()
		{
            Debug.Log("Entering Spiral Shoot State");
			_shotTimer = 0f;
			_currentShotCount = 0;
			_spiralPattern = new SpiralPattern(_shootPatternConfig.AngleIncrement);
		}

		public override void Exit()
		{
			// Logic for exiting the Spiral Shoot state
		}

		public override void UpdateState()
		{
			_shotTimer += Time.deltaTime;
			if (_shotTimer >= _shootPatternConfig.ShotInterval)
			{
				_shotTimer = 0f;
				_currentShotCount++;
				Shoot();
				if (_currentShotCount >= _shootPatternConfig.NumberOfShots)
				{
					OnStateFinished();
				}
			}
		}

		private void Shoot()
		{
			List<Projectile> projectiles = new List<Projectile>();
			for (int i = 0; i < _shootPatternConfig.ProjectilesPerShot; i++)
			{
				var projectile = EnemyProjectileManager.Instance.GetFreeProjectile();
				if (projectile != null)
				{
					projectiles.Add(projectile);
				}
			}

			_spiralPattern.Shoot(
                projectiles, 
                transform.position, 
                _shootPatternConfig.ProjectileSpeed, 
                _shootPatternConfig.ProjectileDamage);
		}
	}
}
