using UnityEngine;

public abstract class Health_UI : MonoBehaviour
{
    public abstract void GameUpdate(int currentHealth, int maxHealth);
}
