using UnityEngine;

[CreateAssetMenu(fileName = "RadialShootStateConfig", menuName = "Enemy State Configs/Radial Shoot State Config")]
public class RadialShootStateConfig : ScriptableObject
{
    [SerializeField] private int numberOfShots = 8;
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float shotInterval = 0.5f;

	public int NumberOfShots => numberOfShots;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ShotInterval => shotInterval;
}
