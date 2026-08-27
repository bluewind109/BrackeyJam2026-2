using UnityEngine;

namespace ShootPatterns
{
	public class RadialPattern : Pattern, IStaticPattern
	{
		private RadialPatternConfig _config;
		private float _angleStep = 0f;
		private float _alternateAngle = 0f;
		private bool _isAlternateAngle = false;

		public RadialPattern(RadialPatternConfig config)
		{
			_config = config;
			_angleStep = 360f / _config.ProjectilesPerShot;
			_alternateAngle = _angleStep / 2f;
		}

		public void Shoot(Vector3 position)
		{
			if (_config == null)
			{
				Debug.LogError("RadialPattern.Shoot called with null config.");
				return;
			}
			if (_config.ProjectilesPerShot <= 0)
			{
				Debug.LogWarning("RadialPattern.Shoot called with no projectiles to spawn.");
				return;
			}

			if (EnemyProjectileManager.Instance == null)
			{
				Debug.LogError("EnemyProjectileManager instance is missing.");
				return;
			}

			float startAngle = _isAlternateAngle ? _alternateAngle : 0f;

			for (int i = 0; i < _config.ProjectilesPerShot; i++)
			{
				float shotAngle = startAngle + (i * _angleStep);
				float projectileAngleInRadians = shotAngle * Mathf.Deg2Rad;
				Vector3 projectileMoveDirection = new Vector3(
					Mathf.Sin(projectileAngleInRadians),
					Mathf.Cos(projectileAngleInRadians),
					0f
				).normalized;

				EnemyProjectileManager.Instance.SpawnProjectile(
					_config.ProjectileDamage,
					position,
					projectileMoveDirection,
					_config.ProjectileSpeed
				);
			}

			_isAlternateAngle = !_isAlternateAngle;
		}
	}
}
