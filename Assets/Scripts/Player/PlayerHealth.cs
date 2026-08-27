using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health_UI
{
    [SerializeField] private List<Image> healthImages = new List<Image>();

    [SerializeField] private Color noHealthColor;
    [SerializeField] private Color fullHealthColor;

	public override void GameUpdate(int currentHealth, int maxHealth)
	{
		for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < currentHealth)
            {
                healthImages[i].color = fullHealthColor;
            }
            else
            {
                healthImages[i].color = noHealthColor;
            }
        }
	}
}
