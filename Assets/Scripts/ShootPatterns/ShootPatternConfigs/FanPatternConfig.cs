using UnityEngine;

[CreateAssetMenu(fileName = "FanPatternConfig", menuName = "Shoot Patterns/Fan Pattern Config")]
public class FanPatternConfig : ShootPatternConfig, IShotBasedPattern
{
	[Header("Fan Pattern Settings")]
	[SerializeField] private int projectilesPerShot = 5;
	[SerializeField] private float spreadAngle = 45f;
	[SerializeField] private int numberOfShots = 8;

	public int ProjectilesPerShot => projectilesPerShot;
	public float SpreadAngle => spreadAngle;
	public int NumberOfShots => numberOfShots;
}
