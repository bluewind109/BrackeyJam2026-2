using UnityEngine;

[CreateAssetMenu(fileName = "DashStateConfig", menuName = "Enemy State Configs/Dash State Config")]
public class DashStateConfig : ScriptableObject
{
	[SerializeField] private float delayDuration = 1f;
	[SerializeField] private float dashSpeed = 10f;
	[SerializeField] private float dashDistance = 10f;

	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float projectileSpeed = 2.5f;
	[SerializeField] private float shotInterval = 0.25f;

	public float DelayDuration => delayDuration;
	public float DashSpeed => dashSpeed;
	public float DashDistance => dashDistance;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ProjectileSpeed => projectileSpeed;
	public float ShotInterval => shotInterval;
}
