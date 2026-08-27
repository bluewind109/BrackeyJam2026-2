using UnityEngine;

public abstract class ShootPatternConfig : ScriptableObject
{
	[Header("Projectile Settings")]
	[SerializeField] private int projectileDamage = 1;
	[SerializeField] private float projectileSpeed = 2.5f;
	[SerializeField] private float shotInterval = 0.25f;
	
	public int ProjectileDamage => projectileDamage;
	public float ProjectileSpeed => projectileSpeed;
	public float ShotInterval => shotInterval;
}

public interface IShotBasedPattern
{
	public int NumberOfShots { get; }
}

public interface ITimeBasedPattern
{
	public float Duration { get; }
}
