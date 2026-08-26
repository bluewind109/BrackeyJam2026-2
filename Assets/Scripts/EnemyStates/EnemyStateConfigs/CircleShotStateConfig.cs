using UnityEngine;

[CreateAssetMenu(fileName = "CircleShotStateConfig", menuName = "Enemy State Configs/Circle Shot State Config")]
public class CircleShotStateConfig : ScriptableObject
{
    [SerializeField] private int numberOfShots = 8;
	[SerializeField] private int projectilesPerShot = 10;
	[SerializeField] private float shotInterval = 0.5f;

	public int NumberOfShots => numberOfShots;
	public int ProjectilesPerShot => projectilesPerShot;
	public float ShotInterval => shotInterval;
}
