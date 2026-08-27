using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralPatternConfig", menuName = "Shoot Patterns/Spiral Pattern Config")]
public class SpiralPatternConfig : ShootPatternConfig
{
	[Header("Spiral Pattern Settings")]
	[SerializeField] private float angleIncrement = 15f;

	public float AngleIncrement => angleIncrement;
}
