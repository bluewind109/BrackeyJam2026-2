using System.Collections.Generic;
using UnityEngine;
using System;

namespace ShootPatterns
{
	public abstract class Pattern
	{
		public event Action OnShootComplete;
	}

	public class SpiralPattern : Pattern
	{
		public void Shoot(List<Projectile> projectiles, Vector3 position, float projectileSpeed, float startAngle = 0f)
		{
			float angleStep = 360f / projectiles.Count;
			float angle = startAngle;

			for (int i = 0; i < projectiles.Count; i++)
			{
				float projectileDirXPosition = position.x + Mathf.Sin((angle * Mathf.PI) / 180);
				float projectileDirYPosition = position.y + Mathf.Cos((angle * Mathf.PI) / 180);

				Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
				Vector3 projectileMoveDirection = (projectileVector - position).normalized;
				projectiles[i].Initialize(projectileMoveDirection, projectileSpeed, "Player");

				angle += angleStep;
			}
		}
	}
}
