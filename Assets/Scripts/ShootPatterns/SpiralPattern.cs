using UnityEngine;

namespace ShootPatterns
{
	public class SpiralPattern : Pattern
	{
		private float _currentAngle = 0f;
		private float _angleIncrement = 0f;

		public SpiralPattern(float angleIncrement = 10f)
		{
			_angleIncrement = angleIncrement;
		}

		public void Shoot(
			int projectileCount,
			Vector3 position,
			float projectileSpeed,
			int projectileDamage)
		{
			if (projectileCount <= 0)
			{
				Debug.LogWarning("SpiralPattern.Shoot called with no projectiles to spawn.");
				return;
			}

			if (EnemyProjectileManager.Instance == null)
			{
				Debug.LogError("EnemyProjectileManager instance is missing.");
				return;
			}

			float angleStep = 360f / projectileCount;
			float angle = _currentAngle;

			for (int i = 0; i < projectileCount; i++)
			{
				float projectileDirXPosition = position.x + Mathf.Sin((angle * Mathf.PI) / 180);
				float projectileDirYPosition = position.y + Mathf.Cos((angle * Mathf.PI) / 180);

				Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
				Vector3 projectileMoveDirection = (projectileVector - position).normalized;
				EnemyProjectileManager.Instance.SpawnProjectile(
					projectileDamage,
					position,
					projectileMoveDirection,
					projectileSpeed
				);

				angle += angleStep;
			}
			_currentAngle += _angleIncrement;
		}
	}
}
