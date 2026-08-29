using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health_UI
{
    [SerializeField] private List<Image> healthImages = new List<Image>();

    [SerializeField] private Sprite noHealth;
    [SerializeField] private Sprite fullHealth;

	public override void GameUpdate(int currentHealth, int maxHealth)
	{
		for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < currentHealth)
            {
                healthImages[i].sprite = fullHealth;
            }
            else
            {
                healthImages[i].sprite = noHealth;
            }
        }
	}
}
