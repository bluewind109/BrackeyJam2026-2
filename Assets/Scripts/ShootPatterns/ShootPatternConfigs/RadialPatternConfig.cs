using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RadialPatternConfig", menuName = "Shoot Patterns/Radial Pattern Config")]
public class RadialPatternConfig : ShootPatternConfig
{
	[SerializeField] private int damage = 1;
    [SerializeField] private int numberOfShots = 8;
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float projectileSpeed = 2.5f;
	[SerializeField] private float shotInterval = 0.25f;

	public int Damage => damage;
	public int NumberOfShots => numberOfShots;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ProjectileSpeed => projectileSpeed;
	public float ShotInterval => shotInterval;
}
