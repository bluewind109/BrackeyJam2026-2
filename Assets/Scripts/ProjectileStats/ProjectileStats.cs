using UnityEngine;

public class ProjectileStats : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private int speed;
    [SerializeField] private float lifetime;

    public int Damage => damage;
    public int Speed => speed;
    public float Lifetime => lifetime;
}
