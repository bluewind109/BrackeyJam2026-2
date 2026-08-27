using UnityEngine;

namespace ShootPatterns
{
	public class SidewayPattern : Pattern, IDirectionalPattern
	{
		private SidewayPatternConfig _config;

		public SidewayPattern(SidewayPatternConfig config)
		{
			_config = config;
		}

		// Shoot on both sides of the direction vector
		public void Shoot(Vector3 position, Vector3 direction)
		{
			if (_config == null)
			{
				Debug.LogError("SidewayPattern.Shoot called with null config.");
				return;
			}

			if (EnemyProjectileManager.Instance == null)
			{
				Debug.LogError("EnemyProjectileManager instance is missing.");
				return;
			}

			// Direction is left and right of the given direction vector
			Vector3 shootDirection = Vector3.Cross(direction, Vector3.forward).normalized;
			for (int i = 0; i < 2; i++)
			{
				Vector3 spawnPosition = position + (i == 0 ? shootDirection : -shootDirection);
				EnemyProjectileManager.Instance.SpawnProjectile(
					_config.ProjectileDamage,
					spawnPosition,
					direction,
					_config.ProjectileSpeed
				);
			}
		}
	}
}
