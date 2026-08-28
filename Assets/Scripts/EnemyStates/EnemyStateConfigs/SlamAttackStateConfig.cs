using UnityEngine;

[CreateAssetMenu(fileName = "SlamAttackStateConfig", menuName = "Enemy State Configs/Slam Attack State Config")]
public class SlamAttackStateConfig : ScriptableObject
{
    [SerializeField] private float slamAttackDelay = 1f;
	[SerializeField] private float slamAttackPersistenceDuration = 0.1f;
	[SerializeField] private int slamAttackDamage = 1;
	[SerializeField] private float slamAttackRadius = 16f;

	public float SlamAttackDelay => slamAttackDelay;
	public float SlamAttackPersistenceDuration => slamAttackPersistenceDuration;
	public int SlamAttackDamage => slamAttackDamage;
	public float SlamAttackRadius => slamAttackRadius;
}
