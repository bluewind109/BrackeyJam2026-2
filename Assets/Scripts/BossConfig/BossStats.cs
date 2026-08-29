using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Boss Stats")]
public class BossStats : ScriptableObject
{
    [SerializeField] private int maxHealth = 100;

	public int MaxHealth => maxHealth;
}
