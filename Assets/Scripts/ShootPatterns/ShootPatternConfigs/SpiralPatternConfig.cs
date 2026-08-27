using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralPatternConfig", menuName = "Shoot Patterns/Spiral Pattern Config")]
public class SpiralPatternConfig : ShootPatternConfig, IShotBasedPattern
{
	[Header("Spiral Pattern Settings")]
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private int numberOfShots = 8;
	[SerializeField] private float angleIncrement = 15f;

	public int ProjectilesPerShot => projectilesPerShot;
	public int NumberOfShots => numberOfShots;
	public float AngleIncrement => angleIncrement;
}
