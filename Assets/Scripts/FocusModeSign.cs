using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FocusModeSign : MonoBehaviour
{
    [SerializeField] private Image _focusModeSignImage;

    private Vector3 _originalPosition;

    void Awake()
    {
        if (_focusModeSignImage != null)
        {
            _originalPosition = _focusModeSignImage.rectTransform.localPosition;
        }
    }

    public void UpdateSprite(Sprite newSprite)
    {
        if (_focusModeSignImage == null) return;
        _focusModeSignImage.sprite = newSprite;
    }

    public void ShakeImage()
    {
        if (_focusModeSignImage == null) return;

        float shakeDuration = 0.2f;
        float shakeStrength = 10f;
        int vibrato = 10;

        _focusModeSignImage.rectTransform.DOShakePosition(shakeDuration, shakeStrength, vibrato)
            .OnComplete(() => _focusModeSignImage.rectTransform.localPosition = _originalPosition);
    }
}
