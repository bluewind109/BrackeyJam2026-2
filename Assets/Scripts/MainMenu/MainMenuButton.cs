using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuButton : MonoBehaviour
{
    public event Action onPressed;

    [SerializeField] private MainMenuActionType actionType;
    [SerializeField] private Image activeBg;
    [SerializeField] private TextMeshProUGUI buttonText;

    private CanvasGroup canvasGroup;

    private bool isActive = false;
    public MainMenuActionType ActionType => actionType;

    public void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup) PlayTweenAlphaLoop(1f, 0f, 1f);
    }

    public void PlayTweenAlphaLoop(float duration, float minAlpha, float maxAlpha)
    {
        canvasGroup?.DOFade(minAlpha, duration).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetActive(bool active)
    {
        isActive = active;
        activeBg.gameObject.SetActive(active);
        buttonText.color = isActive ? Color.black : Color.white;
    }
}
