using UnityEngine;

[CreateAssetMenu(fileName = "SpiralShootStateConfig", menuName = "Enemy State Configs/Spiral Shoot State Config")]
public class SpiralShootStateConfig : ScriptableObject
{
	[SerializeField] private int numberOfShots = 8;
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float shotInterval = 0.5f;
	[SerializeField] private float angleIncrement = 10f;

	public int NumberOfShots => numberOfShots;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ShotInterval => shotInterval;
	public float AngleIncrement => angleIncrement;
}
