using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Health_UI
{
    [SerializeField] private Image fill;

	public override void GameUpdate(int currentHealth, int maxHealth)
    {
        fill.fillAmount = (float)currentHealth / maxHealth;
    }
}
