using UnityEngine;

[CreateAssetMenu(fileName = "MeteorRainStateConfig", menuName = "Enemy State Configs/Meteor Rain State Config")]
public class MeteorRainStateConfig : EnemyStateConfig
{
	[SerializeField] private int numberOfMeteors = 8;
	[SerializeField] private float minSpawnRadius = 0.9f;
	[SerializeField] private float maxSpawnRadius = 2.4f;
	[SerializeField] private float baseDelayDuration = 1f;
	[SerializeField] private float spawnRadius = 1.5f;
	[SerializeField] private float spawnDelayJitter = 0.35f;
	[SerializeField] private float spawnStaggerMin = 0.03f;
	[SerializeField] private float spawnStaggerMax = 0.12f;
	[SerializeField] private float minSpawnDistanceBetweenAttacks = 0.65f;
	[SerializeField] private int meteorDamage = 1;

	public int NumberOfMeteors => numberOfMeteors;
	public float MinSpawnRadius => minSpawnRadius;
	public float MaxSpawnRadius => maxSpawnRadius;
	public float BaseDelayDuration => baseDelayDuration;
	public float SpawnRadius => spawnRadius;
	public float SpawnDelayJitter => spawnDelayJitter;
	public float SpawnStaggerMin => spawnStaggerMin;
	public float SpawnStaggerMax => spawnStaggerMax;
	public float MinSpawnDistanceBetweenAttacks => minSpawnDistanceBetweenAttacks;
	public int MeteorDamage => meteorDamage;
}
