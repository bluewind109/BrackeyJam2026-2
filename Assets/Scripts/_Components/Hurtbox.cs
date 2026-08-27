using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private Health _health;

    public void Initialize(Health health)
    {
        _health = health;
    }

    public void TakeDamage(int damage)
    {
        // Debug.Log($"Hurtbox took {damage} damage.");
        _health?.TakeDamage(damage);
    }
}
