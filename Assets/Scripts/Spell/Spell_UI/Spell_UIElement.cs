using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Spell_UIElement : MonoBehaviour
{
    [SerializeField] private Image _cooldownBar;
    [SerializeField] private Slider _experienceBar;
    [SerializeField] private Image _levelFrame;

    [SerializeField] private Sprite spriteLevel1;
    [SerializeField] private Sprite spriteLevel2;
    [SerializeField] private Sprite spriteLevel3;

    [SerializeField] private List<Image> _inputImages = new List<Image>();

    public void Initialize(int level, List<InputDirection> inputDirections)
    {
        UpdateLevelFrame(level);
        for (int i = 0; i < _inputImages.Count; i++)
        {
            if (i < inputDirections.Count)
            {
                _inputImages[i].sprite = InputSpriteManager.Instance.GetInputSprite(inputDirections[i]);
                _inputImages[i].enabled = true;
            }
            else
            {
                _inputImages[i].enabled = false;
            }
        }
    }

    private void UpdateLevelFrame(int level)
    {
        switch (level)
        {
            case 1:
                _levelFrame.sprite = spriteLevel1;
                break;
            case 2:
                _levelFrame.sprite = spriteLevel2;
                break;
            case 3:
                _levelFrame.sprite = spriteLevel3;
                break;
            default:
                Debug.LogWarning($"Invalid level {level} for spell UI element.");
                break;
        }
    }

    public void UpdateExperience(float experience, float experienceRequired)
    {
        _experienceBar.value = Mathf.Clamp01(experience / experienceRequired);
    }

    public void UpdateCooldown(float cooldown, float cooldownTimer)
    {
        _cooldownBar.fillAmount = Mathf.Clamp01(cooldownTimer / cooldown);
    }
}
