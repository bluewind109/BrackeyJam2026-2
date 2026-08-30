using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] UnityEngine.Events.UnityEvent<int> onTakeDamage;
    private Health _health;

    public void Initialize(Health health)
    {
        _health = health;
    }

    public void ToggleCollider(bool isEnabled)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = isEnabled;
        }
    }

    public void TakeDamage(int damage)
    {
        // Debug.Log($"Hurtbox took {damage} damage.");
        _health?.TakeDamage(damage);
        onTakeDamage?.Invoke(damage);
    }
}
