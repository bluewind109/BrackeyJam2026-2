using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Boss Stats")]
public class BossStats : ScriptableObject
{
    [SerializeField] private int maxHealth = 100;
	[SerializeField] private float moveSpeed = 4f;

	public int MaxHealth => maxHealth;
	public float MoveSpeed => moveSpeed;
}
