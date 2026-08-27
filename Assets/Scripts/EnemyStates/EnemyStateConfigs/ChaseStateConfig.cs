using UnityEngine;

[CreateAssetMenu(fileName = "ChaseStateConfig", menuName = "Enemy State Configs/Chase State Config")]
public class ChaseStateConfig : ScriptableObject
{
    [SerializeField] private float chaseSpeed = 4f;
	[SerializeField] private float chaseDuration = 2f;

	public float ChaseSpeed => chaseSpeed;
	public float ChaseDuration => chaseDuration;
}
