using System.Collections.Generic;
using UnityEngine;

namespace ShootPatterns
{
	public class SpiralPattern : Pattern
	{
		private float _currentAngle = 0f;

		public void Shoot(
			List<Projectile> projectiles,
			Vector3 position,
			float projectileSpeed,
			float angleIncrement)
		{
			float angleStep = 360f / projectiles.Count;
			float angle = _currentAngle;

			for (int i = 0; i < projectiles.Count; i++)
			{
				float projectileDirXPosition = position.x + Mathf.Sin((angle * Mathf.PI) / 180);
				float projectileDirYPosition = position.y + Mathf.Cos((angle * Mathf.PI) / 180);

				Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
				Vector3 projectileMoveDirection = (projectileVector - position).normalized;
				projectiles[i].Initialize(projectileMoveDirection, projectileSpeed, "Player");

				angle += angleStep;
			}
			_currentAngle += angleIncrement;
			Debug.Log($"Current Angle: {_currentAngle}");
		}
	}
}
