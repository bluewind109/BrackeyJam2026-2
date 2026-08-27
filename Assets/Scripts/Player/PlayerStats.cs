using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int maxHealth = 10;
	[SerializeField] private float moveSpeed = 3f;
	[SerializeField] private int baseDamage = 3;
	[SerializeField] private float focusCooldown = 3f;

	public int MaxHealth => maxHealth;
	public float MoveSpeed => moveSpeed;
	public int BaseDamage => baseDamage;
	public float FocusCooldown => focusCooldown;
}
