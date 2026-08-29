using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

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
}
