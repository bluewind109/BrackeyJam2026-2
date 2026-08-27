using UnityEngine;

public abstract class ShootPatternConfig : ScriptableObject
{
	[Header("Projectile Settings")]
	[SerializeField] private int projectileDamage = 1;
    [SerializeField] private int numberOfShots = 8;
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float projectileSpeed = 2.5f;
	[SerializeField] private float shotInterval = 0.25f;
	
	public int ProjectileDamage => projectileDamage;
    public int NumberOfShots => numberOfShots;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ProjectileSpeed => projectileSpeed;
	public float ShotInterval => shotInterval;
}
