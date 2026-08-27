using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralPatternConfig", menuName = "Shoot Patterns/Spiral Pattern Config")]
public class SpiralPatternConfig : ShootPatternConfig
{
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float projectileSpeed = 2.5f;
	[SerializeField] private float shotInterval = 0.25f;
	[SerializeField] private float angleIncrement = 15f;

	public int ProjectilesPerShot => projectilesPerShot;
	public float ProjectileSpeed => projectileSpeed;
	public float ShotInterval => shotInterval;
}
