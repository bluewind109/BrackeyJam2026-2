using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Alchemy.Serialization;
[AlchemySerialize]
public partial class EnemyDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animatorPlayer;
    [AlchemySerializeField, NonSerialized] private Dictionary<string, MMF_Player> _spellCasting = new();
    private Enemy _enemy;

    public void Initialize(Enemy enemy)
    {
        if (enemy == null)
        {
            return;
        }

        _enemy = enemy;
    }

    public void UpdateMoving(Vector2 direction)
    {
        if (direction.x < -0.1f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (direction.x > 0.1f)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void PlayIdleAnimation()
    {
        string animationName = $"Lv{_enemy.CurrentPhase}_Idle";
        _animatorPlayer.Play(animationName);
    }

    public void PlayPrepareAnimation()
    {
        string animationName = $"Lv{_enemy.CurrentPhase}_Skill_Prepare";
        _animatorPlayer.Play(animationName);
    }

    public void PlayCastAnimation(string spellName)
    {
        string animationName = $"Lv{_enemy.CurrentPhase}_Skill_Cast";
        _animatorPlayer.Play(animationName);

        if (_spellCasting.TryGetValue(spellName, out MMF_Player castingFeedback))
        {
            castingFeedback?.PlayFeedbacks();
        }
        else
        {
            Debug.LogWarning($"Casting feedback for spell name {spellName} not found.");
        }
    }

    public void PlayTransformAnimation()
    {
        string animationName = $"Lv{_enemy.CurrentPhase}_Transform";
        _animatorPlayer.Play(animationName);
    }
}
