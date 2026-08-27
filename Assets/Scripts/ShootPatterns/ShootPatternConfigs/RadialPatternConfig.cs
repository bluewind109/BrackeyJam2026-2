using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RadialPatternConfig", menuName = "Shoot Patterns/Radial Pattern Config")]
public class RadialPatternConfig : ShootPatternConfig, IShotBasedPattern
{
	[Header("Radial Pattern Settings")]
	[SerializeField] private int projectilesPerShot = 10;
    [SerializeField] private int numberOfShots = 8;

	public int ProjectilesPerShot => projectilesPerShot;
    public int NumberOfShots => numberOfShots;
}
