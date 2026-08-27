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
				float projectileDirXPosition = position.x + Mathf.Sin((startAngle * Mathf.PI) / 180);
				float projectileDirYPosition = position.y + Mathf.Cos((startAngle * Mathf.PI) / 180);

				Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
				Vector3 projectileMoveDirection = (projectileVector - position).normalized;
				EnemyProjectileManager.Instance.SpawnProjectile(
					_config.ProjectileDamage,
					position,
					projectileMoveDirection,
					_config.ProjectileSpeed
				);

				startAngle += _angleStep;
			}

			_isAlternateAngle = !_isAlternateAngle;
		}
	}
}
