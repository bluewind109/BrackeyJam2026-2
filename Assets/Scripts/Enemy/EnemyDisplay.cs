using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animatorPlayer;

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

    public void PlayCastAnimation()
    {
        string animationName = $"Lv{_enemy.CurrentPhase}_Skill_Cast";
        _animatorPlayer.Play(animationName);
    }

    public void PlayTransformAnimation()
    {
        string animationName = $"Lv{_enemy.CurrentPhase}_Transform";
        _animatorPlayer.Play(animationName);
    }
}
