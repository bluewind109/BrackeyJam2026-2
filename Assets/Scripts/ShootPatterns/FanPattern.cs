using UnityEngine;

namespace ShootPatterns
{
	public class FanPattern : Pattern, IDirectionalPattern
	{
		private FanPatternConfig _config;

		public FanPattern(FanPatternConfig config)
		{
			_config = config;
		}

		public void Shoot(Vector3 position, Vector3 direction)
		{
			if (_config == null)
			{
				Debug.LogError("FanPattern.Shoot called with null config.");
				return;
			}

			if (EnemyProjectileManager.Instance == null)
			{
				Debug.LogError("EnemyProjectileManager instance is missing.");
				return;
			}

			float angleStep = _config.SpreadAngle / (_config.ProjectilesPerShot - 1);
			float startAngle = -_config.SpreadAngle / 2f;

			for (int i = 0; i < _config.ProjectilesPerShot; i++)
			{
				float shotAngle = startAngle + (i * angleStep);
				Vector3 rotatedDirection = Quaternion.Euler(0, 0, shotAngle) * direction.normalized;

				EnemyProjectileManager.Instance.SpawnProjectile(
					_config.ProjectileDamage,
					position,
					rotatedDirection,
					_config.ProjectileSpeed
				);
			}
		}
	}
}