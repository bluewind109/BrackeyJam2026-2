using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action<int> onHealthChanged;
    public Action onDeath;

    [SerializeField] private Health_UI healthUI;

    private int maxHealth = 1;
    private int currentHealth;

    public void Initialize(int health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        healthUI?.GameUpdate(currentHealth, maxHealth);
    }
    
    public void TakeDamage(int damage)
    {
        // Debug.Log($"<color=red>Damage taken: {damage}</color>");
        currentHealth -= damage;
        onHealthChanged?.Invoke(currentHealth);
        healthUI?.GameUpdate(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        onDeath?.Invoke();
    }
}
