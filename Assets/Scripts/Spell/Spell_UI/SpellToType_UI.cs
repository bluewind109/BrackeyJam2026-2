using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellToType_UI : MonoBehaviour
{
    [SerializeField] private Image _spellIcon;
    [SerializeField] private Image _spellFrame;
    [SerializeField] private List<Image> _inputSequenceImage;

    private bool _isOnCooldown;
    public bool IsOnCooldown => _isOnCooldown;

    private List<InputDirection> _currentInputDirections = new List<InputDirection>();

    public void UpdateSpellToTypeUI(
        Sprite spellIcon,
        Sprite spellFrame,
        List<InputDirection> inputDirections,
        bool isOnCooldown)
    {
        _currentInputDirections = new List<InputDirection>(inputDirections);

        _spellIcon.sprite = spellIcon;
        _spellFrame.sprite = spellFrame;
        _isOnCooldown = isOnCooldown;
        ResetInputSprites();
    }

    private void ResetInputSprites()
    {
        for (int i = 0; i < _inputSequenceImage.Count; i++)
        {
            if (i < _currentInputDirections.Count)
            {
                InputDirection inputDirection = _currentInputDirections[i];
                Sprite inputSprite = InputSpriteManager.Instance.GetInputSprite_FocusMode(inputDirection);
                _inputSequenceImage[i].sprite = inputSprite;
                _inputSequenceImage[i].gameObject.SetActive(true);
            }
            else
            {
                _inputSequenceImage[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateInputSequence(List<InputDirection> playerInputDirections)
    {
        for (int i = 0; i < _inputSequenceImage.Count; i++)
        {
            if (i < playerInputDirections.Count)
            {
                InputDirection inputDirection = playerInputDirections[i];
                Sprite inputSprite = InputSpriteManager.Instance.GetInputSprite_FocusMode_Typed(inputDirection);
                _inputSequenceImage[i].sprite = inputSprite;
            }
        }
    }

    public void ResetInputSequence()
    {
        ResetInputSprites();
    }
}

public class SpellToType_UI_Data
{
    public SpellType SpellType;
    public Sprite SpellIcon;
    public int Level;
    public List<InputDirection> InputDirections;
    public bool IsOnCooldown;

    public SpellToType_UI_Data(
        SpellType spellType,
        Sprite spellIcon,
        int level,
        List<InputDirection> inputDirections,
        bool isOnCooldown)
    {
        SpellType = spellType;
        SpellIcon = spellIcon;
        Level = level;
        InputDirections = inputDirections;
        IsOnCooldown = isOnCooldown;
    }
}
