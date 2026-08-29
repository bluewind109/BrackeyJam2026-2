using UnityEngine;

public class PredictionLine : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _lineSprite;

    public void GameUpdate(float _timer, float _duration)
    {
        Vector3 scale = _lineSprite.transform.localScale;
        scale.x = Mathf.Clamp01(_timer / _duration);
        _lineSprite.transform.localScale = scale;
    }
}
