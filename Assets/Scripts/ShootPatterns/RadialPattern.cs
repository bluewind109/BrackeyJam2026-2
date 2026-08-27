using UnityEngine;

namespace ShootPatterns
{
	public class RadialPattern : Pattern, IStaticPattern
	{
		private RadialPatternConfig _config;

		public RadialPattern(RadialPatternConfig config)
		{
			_config = config;
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

			float angleStep = 360f / _config.ProjectilesPerShot;
			float angle = 0f;

			for (int i = 0; i < _config.ProjectilesPerShot; i++)
			{
				float projectileDirXPosition = position.x + Mathf.Sin((angle * Mathf.PI) / 180);
				float projectileDirYPosition = position.y + Mathf.Cos((angle * Mathf.PI) / 180);

				Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
				Vector3 projectileMoveDirection = (projectileVector - position).normalized;
				EnemyProjectileManager.Instance.SpawnProjectile(
					_config.ProjectileDamage,
					position,
					projectileMoveDirection,
					_config.ProjectileSpeed
				);

				angle += angleStep;
			}
		}
	}
}
