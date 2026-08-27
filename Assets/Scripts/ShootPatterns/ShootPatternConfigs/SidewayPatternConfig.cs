using UnityEngine;

[CreateAssetMenu(fileName = "SidewayPatternConfig", menuName = "Shoot Patterns/Sideway Pattern Config")]
public class SidewayPatternConfig : ShootPatternConfig, ITimeBasedPattern
{
	[Header("Sideway Pattern Settings")]
	[SerializeField] private float duration = 2f;

	public float Duration => duration;
}
