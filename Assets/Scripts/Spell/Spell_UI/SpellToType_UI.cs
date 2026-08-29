using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellToType_UI : MonoBehaviour
{
    [SerializeField] private Image _spellIcon;
    [SerializeField] private Image _spellFrame;
    [SerializeField] private List<Image> _inputSequenceImage;

    public void UpdateSpellToTypeUI(
        Sprite spellIcon, 
        Sprite spellFrame, 
        List<InputDirection> inputDirections)
    {
        _spellIcon.sprite = spellIcon;
        _spellFrame.sprite = spellFrame;

        for (int i = 0; i < _inputSequenceImage.Count; i++)
        {
            if (i < inputDirections.Count)
            {
                InputDirection inputDirection = inputDirections[i];
                Sprite inputSprite = InputSpriteManager.Instance.GetInputSprite(inputDirection);
                _inputSequenceImage[i].sprite = inputSprite;
                _inputSequenceImage[i].gameObject.SetActive(true);
            }
            else
            {
                _inputSequenceImage[i].gameObject.SetActive(false);
            }
        }
    }
}

public class SpellToType_UI_Data
{
    public SpellType SpellType;
    public Sprite SpellIcon;
    public int Level;
    public List<InputDirection> InputDirections;

    public SpellToType_UI_Data(
        SpellType spellType,
        Sprite spellIcon,
        int level,
        List<InputDirection> inputDirections)
    {
        SpellType = spellType;
        SpellIcon = spellIcon;
        Level = level;
        InputDirections = inputDirections;
    }
}
