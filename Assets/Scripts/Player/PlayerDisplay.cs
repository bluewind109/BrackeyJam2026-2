using UnityEngine;
using System;
using System.Collections.Generic;
using Alchemy.Serialization;
[AlchemySerialize]
public partial class PlayerDisplay : MonoBehaviour
{
    private enum MovementState { Idle, Move }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animatorPlayer;
    [SerializeField] private Animator _animatorVFX;
    [SerializeField] private SpriteRenderer _spriteRendererFocusVFX;
    [AlchemySerializeField, NonSerialized] private Dictionary<SpellType, Color> _spellFocusColors = new();
    [AlchemySerializeField, NonSerialized] private Dictionary<SpellType, GameObject> _maxLevelSpellVFXs = new();
    [AlchemySerializeField, NonSerialized] private Dictionary<SpellType, GameObject> _overHeatSpellVFXs = new();

    private MovementState _currentState = MovementState.Idle;

    public bool IsSpriteRendererVisible => _spriteRenderer.color.a >= 1f;

    public void ToggleDisplay(bool isVisible)
    {
        _spriteRenderer.color = isVisible ? Color.white : new Color(1f, 1f, 1f, 0f);
    }

    public void UpdateMoving(Vector2 direction)
    {
        if (direction.x > 0.1f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (direction.x < -0.1f)
        {
            _spriteRenderer.flipX = true;
        }

        MovementState newState = direction == Vector2.zero ? MovementState.Idle : MovementState.Move;
        if (newState == _currentState) return;

        _currentState = newState;
        _animatorPlayer.Play(newState == MovementState.Move ? "Move" : "Idle");
    }

    public void PlayCastSpellAnimation()
    {
        _animatorPlayer.Play("Cast");
        _animatorVFX.Play("Cast");
    }

    public void TriggerFocusVFXAnimation(SpellInfo spellInfo)
    {
        if (spellInfo == null) return;
        if (_spellFocusColors.TryGetValue(spellInfo.SpellType, out Color focusColor))
        {
            _spriteRendererFocusVFX.color = focusColor;
        }
        else
        {
            Debug.LogWarning($"Focus color for spell type {spellInfo.SpellType} not found.");
            _spriteRendererFocusVFX.color = Color.white; // Default color if not found
        }
        _animatorVFX.Play("Focus");
    }

    public void TriggerMaxLevelVFX(SpellType spellType)
    {
        if (_maxLevelSpellVFXs.TryGetValue(spellType, out GameObject maxLevelVFX))
        {
            maxLevelVFX.SetActive(true);

        }
        else
        {
            Debug.LogWarning($"Max level VFX for spell type {spellType} not found.");
        }
    }

    public void TriggerOverHeatVFX(SpellType spellType)
    {
        if (_overHeatSpellVFXs.TryGetValue(spellType, out GameObject overHeatVFX))
        {
            overHeatVFX.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Overheat VFX for spell type {spellType} not found.");
        }
    }
}
