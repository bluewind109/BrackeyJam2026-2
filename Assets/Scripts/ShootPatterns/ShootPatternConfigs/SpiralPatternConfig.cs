using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralPatternConfig", menuName = "Shoot Patterns/Spiral Pattern Config")]
public class SpiralPatternConfig : ShootPatternConfig
{
	[SerializeField] private int projectileDamage = 1;
	[SerializeField] private int numberOfShots = 8;
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float projectileSpeed = 2.5f;
	[SerializeField] private float shotInterval = 0.25f;
	[SerializeField] private float angleIncrement = 15f;

	public int ProjectileDamage => projectileDamage;
	public int NumberOfShots => numberOfShots;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ProjectileSpeed => projectileSpeed;
	public float ShotInterval => shotInterval;
	public float AngleIncrement => angleIncrement;
}
