using UnityEngine;

[CreateAssetMenu(fileName = "DashStateConfig", menuName = "Enemy State Configs/Dash State Config")]
public class DashStateConfig : EnemyStateConfig
{
	[SerializeField] private float delayDuration = 1f;
	[SerializeField] private float dashSpeed = 10f;
	[SerializeField] private float dashDistance = 10f;
	[SerializeField] private float dashDuration = 2f;

	[SerializeField] private float shotInterval = 0.25f;

	[SerializeField] private RadialPatternConfig radialPatternConfig;
	[SerializeField] private SidewayPatternConfig sidewayPatternConfig;

	public float DelayDuration => delayDuration;
	public float DashSpeed => dashSpeed;
	public float DashDistance => dashDistance;
	public float DashDuration => dashDuration;

	public float ShotInterval => shotInterval;

	public RadialPatternConfig RadialPatternConfig => radialPatternConfig;
	public SidewayPatternConfig SidewayPatternConfig => sidewayPatternConfig;
}
