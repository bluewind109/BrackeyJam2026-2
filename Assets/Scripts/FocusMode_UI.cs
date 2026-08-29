using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FocusMode_UI : MonoBehaviour
{
    public static Action<SpellType, List<InputDirection>> UpdateSpellToType;
    public static Action ResetSpellToType;

    [SerializeField] private Image _focusBarFill;
    [SerializeField] private SpellToType_UI _spellToTypeUI_FireBall;
    [SerializeField] private SpellToType_UI _spellToTypeUI_IceLances;
    [SerializeField] private SpellToType_UI _spellToTypeUI_WindStep;

    [SerializeField] private Sprite _spriteFrame_Lvl1;
    [SerializeField] private Sprite _spriteFrame_Lvl2;
    [SerializeField] private Sprite _spriteFrame_Lvl3;

    private Dictionary<SpellType, SpellToType_UI> _currentSpell_UIs = new Dictionary<SpellType, SpellToType_UI>();

    void Awake()
    {
        _currentSpell_UIs.Add(SpellType.FireBall, _spellToTypeUI_FireBall);
        _currentSpell_UIs.Add(SpellType.IceLances, _spellToTypeUI_IceLances);
        _currentSpell_UIs.Add(SpellType.WindStep, _spellToTypeUI_WindStep);
        UpdateSpellToType += UpdateCurrentSpellToType;
        ResetSpellToType += ResetAllSpellUIs;
    }

    void OnDestroy()
    {
        UpdateSpellToType -= UpdateCurrentSpellToType;
        ResetSpellToType -= ResetAllSpellUIs;
    }

    private void UpdateCurrentSpellToType(
        SpellType spellType,
        List<InputDirection> playerInputDirections)
    {
        if (_currentSpell_UIs.TryGetValue(spellType, out SpellToType_UI spellUI))
        {
            spellUI.UpdateInputSequence(playerInputDirections);

            foreach (var kvp in _currentSpell_UIs)
            {
                if (kvp.Key != spellType)
                {
                    kvp.Value.ToggleAvailability(false);
                }
            }
        }
    }

    private void ResetAllSpellUIs()
    {
        foreach (var kvp in _currentSpell_UIs)
        {
            kvp.Value.ResetInputSequence();

            // Show all spell UIs if not on coooldown
            bool isOnCooldown = kvp.Value.IsOnCooldown;
            kvp.Value.ToggleAvailability(!isOnCooldown);
        }
    }

    public void Show(List<SpellToType_UI_Data> spellDatas)
    {
        if (spellDatas == null || spellDatas.Count == 0) return;

        SpellToType_UI_Data fireBallSpellData = spellDatas.Find(spell => spell.SpellType == SpellType.FireBall);
        UpdateSpellUI(fireBallSpellData, _spellToTypeUI_FireBall);

        SpellToType_UI_Data iceLancesSpellData = spellDatas.Find(spell => spell.SpellType == SpellType.IceLances);
        UpdateSpellUI(iceLancesSpellData, _spellToTypeUI_IceLances);

        SpellToType_UI_Data windStepSpellData = spellDatas.Find(spell => spell.SpellType == SpellType.WindStep);
        UpdateSpellUI(windStepSpellData, _spellToTypeUI_WindStep);

        gameObject.SetActive(true);
    }

    private void UpdateSpellUI(SpellToType_UI_Data spellData, SpellToType_UI spellUI)
    {
        if (spellData == null || spellUI == null) return;

        Sprite frameSprite = GetFrameSpriteForLevel(spellData.Level);
        spellUI.UpdateSpellToTypeUI(
            spellData.SpellIcon,
            frameSprite,
            spellData.InputDirections,
            spellData.IsOnCooldown
        );

        if (spellData.IsOnCooldown)
        {
            spellUI.ToggleAvailability(false);
        }
        else
        {
            spellUI.ToggleAvailability(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateFocusBar(float fillAmount)
    {
        _focusBarFill.fillAmount = Mathf.Clamp01(fillAmount);
    }

    private Sprite GetFrameSpriteForLevel(int level)
    {
        switch (level)
        {
            case 1:
                return _spriteFrame_Lvl1;
            case 2:
                return _spriteFrame_Lvl2;
            case 3:
                return _spriteFrame_Lvl3;
            default:
                return _spriteFrame_Lvl3;
        }
    }
}
