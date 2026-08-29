using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellToType_UI : MonoBehaviour
{
    [SerializeField] private Image _spellIcon;
    [SerializeField] private Image _spellFrame;
    [SerializeField] private Transform _inputSignsParent;
    [SerializeField] private List<FocusModeSign> _inputSigns;

    private bool _isOnCooldown;
    public bool IsOnCooldown => _isOnCooldown;

    private List<InputDirection> _currentInputDirections = new List<InputDirection>();

    void Awake()
    {
        if (_inputSignsParent == null)
        {
            Debug.LogError("Input Signs Parent is not assigned in the inspector.");
            return;
        }

        _inputSigns = new List<FocusModeSign>(_inputSignsParent.GetComponentsInChildren<FocusModeSign>());
    }

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
        for (int i = 0; i < _inputSigns.Count; i++)
        {
            if (i < _currentInputDirections.Count)
            {
                InputDirection inputDirection = _currentInputDirections[i];
                Sprite inputSprite = InputSpriteManager.Instance.GetInputSprite_FocusMode(inputDirection);
                _inputSigns[i].UpdateSprite(inputSprite);
                _inputSigns[i].gameObject.SetActive(true);
            }
            else
            {
                _inputSigns[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateInputSequence(List<InputDirection> playerInputDirections)
    {
        for (int i = 0; i < _inputSigns.Count; i++)
        {
            if (i < playerInputDirections.Count)
            {
                InputDirection inputDirection = playerInputDirections[i];
                Sprite inputSprite = InputSpriteManager.Instance.GetInputSprite_FocusMode_Typed(inputDirection);
                _inputSigns[i].UpdateSprite(inputSprite);
                _inputSigns[i].ShakeImage();
            }
        }
    }

    public void ResetInputSequence()
    {
        ResetInputSprites();
    }

    public void ToggleAvailability(bool isAvailable)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = isAvailable ? 1f : 0.25f;
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
