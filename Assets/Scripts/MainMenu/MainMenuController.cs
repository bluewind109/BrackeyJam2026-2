using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Feedbacks;
public class MainMenuController : MonoBehaviour
{
    public Action onMenuOpened;

    [SerializeField] private MainMenuInput input;
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private Image transitionImage;
    [SerializeField] private Animator slime;
    [SerializeField] private MMFeedbacks feedbacks_roars;
    [SerializeField] private MMFeedbacks feedbacks_enter;

    private List<MainMenuButton> buttons = new List<MainMenuButton>();
    private int selectedIndex = 0;
    private int totalActions => buttons.Count;

    private bool isButtonStartGamePressed = false;

    void Awake()
    {
        buttons = new List<MainMenuButton>(buttonsContainer.GetComponentsInChildren<MainMenuButton>());
        PlayerPrefs.SetInt("CutsceneType", 0);

        if (buttons.Count > 0)
        {
            UpdateSelection();
        }
    }

    void Start()
    {
        slime.Play("Lv3_Idle");
        ShowMenu();
    }

    void OnEnable()
    {
        input.onNavigateUp += OnNavigateUp;
        input.onNavigateDown += OnNavigateDown;
        input.onEnter += OnActionSelect;
        input.onBack += OnBack;
    }

    void OnDisable()
    {
        input.onNavigateUp -= OnNavigateUp;
        input.onNavigateDown -= OnNavigateDown;
        input.onEnter -= OnActionSelect;
        input.onBack -= OnBack;
    }

    void OnDestroy()
    {
        input.onNavigateUp -= OnNavigateUp;
        input.onNavigateDown -= OnNavigateDown;
        input.onEnter -= OnActionSelect;
        input.onBack -= OnBack;
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(i == selectedIndex);
        }
    }

    public void ShowMenu()
    {
        // this.gameObject.SetActive(true);
        input.ToggleInput(true);
        onMenuOpened?.Invoke();
    }

    public void HideMenu()
    {
        // this.gameObject.SetActive(false);
        input.ToggleInput(false);
    }

    private void OnActionSelect()
    {
        var selectedButton = buttons[selectedIndex];

        switch (selectedButton.ActionType)
        {
            case MainMenuActionType.StartGame:
                Debug.Log("Start Game selected");
                if (isButtonStartGamePressed) return;
                isButtonStartGamePressed = true;
                OnButtonStartGamePressed(selectedButton).Forget();
                break;
            case MainMenuActionType.Options:
                Debug.Log("Options selected");
                break;
            case MainMenuActionType.Exit:
                Debug.Log("Exit selected");
                Application.Quit();
                break;
            default:
                Debug.LogWarning("Unhandled action type: " + selectedButton.ActionType);
                break;
        }

    }

    private async UniTask OnButtonStartGamePressed(MainMenuButton button)
    {
        Debug.Log("Start Game button pressed");
        feedbacks_enter?.PlayFeedbacks();
        slime.Play("Lv3_Transform");
        feedbacks_roars?.PlayFeedbacks();
        button.PlayTweenAlphaLoop(0.1f, 0f, 1f);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        
        float transitionDuration = 1f;
        PlayTweenTransitionImage(transitionDuration);
        await UniTask.Delay(TimeSpan.FromSeconds(transitionDuration));
        SceneManager.LoadScene("IntroScene");
    }

    private void PlayTweenTransitionImage(float duration)
    {
        transitionImage.DOFade(1f, duration);
    }

    private void OnBack()
    {
        // HideMenu();
    }


    private void OnNavigateDown()
    {
        if (totalActions == 0) return;

        selectedIndex = (selectedIndex + 1) % totalActions;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, totalActions - 1);
        UpdateSelection();
    }

    private void OnNavigateUp()
    {
        if (totalActions == 0) return;

        selectedIndex = (selectedIndex - 1 + totalActions) % totalActions;
        selectedIndex = Mathf.Clamp(selectedIndex, 0, totalActions - 1);
        UpdateSelection();
    }
}
