using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private CutsceneScript cutsceneScript;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private GameObject choiceMessageGameObject;
    [SerializeField] private TMP_Text choiceMessageText;
    [SerializeField] private CanvasGroup dialogTextCanvasGroup;
    [Header("Dialog Configs")]
    [SerializeField] private CutsceneCollection cutsceneCollection;
    private int currentSentenceIndex = 0;
    DialogConfig currentDialog;
    private SentenceData currentSentenceData;
    private eCutsceneType currentCutsceneType = eCutsceneType.Intro;
    private bool ftuePlayerChoiceMade = true;
    private bool isBlocking = true; // Prevents input during cutscenes or dialogs
    private bool isTyping = false; // True while the current sentence is still being typed out
    private Coroutine typeTextCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // [SerializeField] private eCutsceneType testCutscene = eCutsceneType.Intro;
    private void Start()
    {
        currentCutsceneType = GetCutsceneType();
        CutsceneCheck(currentCutsceneType);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isBlocking)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                CompleteCurrentSentence();
            }
            else
            {
                ftuePlayerChoiceMade = true;
                NextSentence();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isTyping)
            {
                CompleteCurrentSentence();
            }
            else
            {
                ftuePlayerChoiceMade = false;
                NextSentence();
            }
        }
    }

    private void CompleteCurrentSentence()
    {
        if (typeTextCoroutine != null)
        {
            StopCoroutine(typeTextCoroutine);
            typeTextCoroutine = null;
        }
        dialogText.maxVisibleCharacters = dialogText.text.Length;
        isTyping = false;
    }

    private void CutsceneCheck(eCutsceneType cutsceneType)
    {
        currentDialog = cutsceneCollection.GetDialogConfig(cutsceneType);
        switch (cutsceneType)
        {
            case eCutsceneType.Intro:
                PlayCutscene(currentCutsceneType, 
                () => {
                    PlayDialog(currentDialog);
                });
                break;
            case eCutsceneType.Intro_Accept:
                PlayDialog(currentDialog);
                break;
            case eCutsceneType.Intro_Refuse:
                PlayCutscene(currentCutsceneType, 
                () => {
                    PlayDialog(currentDialog);
                });
                break;
            case eCutsceneType.Ending_Death:
                PlayCutscene(currentCutsceneType, 
                () => {
                    PlayDialog(currentDialog);
                });
                break;
            case eCutsceneType.Ending_Exploded:
                PlayCutscene(currentCutsceneType, 
                () => {
                    PlayDialog(currentDialog);
                });
                break;
            case eCutsceneType.Ending_Win:
                PlayDialog(currentDialog);
                break;
            default:
                Debug.LogWarning($"Unhandled cutscene type: {cutsceneType}");
                break;
        }
    }

    private void PlayDialog(DialogConfig dialogConfig)
    {
        if (dialogConfig != null)
        {
            isBlocking = false;
            dialogTextCanvasGroup.DOFade(1, 0.5f);
            currentSentenceIndex = 0;
            currentSentenceData = dialogConfig.SentenceDataList[currentSentenceIndex];
            if (currentSentenceData != null)
            {
                characterNameText.text = currentSentenceData.CharacterName.ToString();
                PlayText(dialogText, currentSentenceData.SentenceText, 0.02f);
                choiceMessageGameObject.SetActive(!string.IsNullOrEmpty(currentSentenceData.ChoiceMessage));
                choiceMessageText.text = currentSentenceData.ChoiceMessage;
            }
        }
    }

    private void NextSentence()
    {
        if (currentDialog != null && currentSentenceIndex < currentDialog.SentenceDataList.Count - 1)
        {
            currentSentenceIndex++;
            currentSentenceData = currentDialog.SentenceDataList[currentSentenceIndex];
            if (currentSentenceData != null)
            {
                characterNameText.text = currentSentenceData.CharacterName.ToString();
                PlayText(dialogText, currentSentenceData.SentenceText, 0.02f);
                choiceMessageGameObject.SetActive(!string.IsNullOrEmpty(currentSentenceData.ChoiceMessage));
                choiceMessageText.text = currentSentenceData.ChoiceMessage;
            }
        }
        else
        {
            Debug.Log("End of dialog.");
            OnDialogFinished();
        }
    }

    private eCutsceneType GetCutsceneType()
    {
        int cutsceneIndex = PlayerPrefs.GetInt("CutsceneType", 0);
        return (eCutsceneType)cutsceneIndex;
    }

    private void OnDialogFinished()
    {
        Debug.Log("Dialog finished.");
        dialogTextCanvasGroup.DOFade(0, 0.5f);
        if (currentCutsceneType == eCutsceneType.Intro)
        {
            OnIntroDialogFinished();
        }
        else if (currentCutsceneType == eCutsceneType.Intro_Accept)
        {
            OnIntroAcceptDialogFinished();
        }
        else if (currentCutsceneType == eCutsceneType.Intro_Refuse)
        {
            OnIntroRefuseDialogFinished();
        }

        else if (currentCutsceneType == eCutsceneType.Ending_Death)
        {
            OnEndingDeathDialogFinished();
        }
        else if (currentCutsceneType == eCutsceneType.Ending_Exploded)
        {
            OnEndingExplodedDialogFinished();
        }
        else if (currentCutsceneType == eCutsceneType.Ending_Win)
        {
            OnEndingWinDialogFinished();
        }
    }

    private void OnIntroDialogFinished()
    {
        Debug.Log("Intro dialog finished.");
        if (ftuePlayerChoiceMade)
        {
            isBlocking = true;
            currentCutsceneType = eCutsceneType.Intro_Accept;
            CutsceneCheck(currentCutsceneType);
        }
        else
        {
            isBlocking = true;
            currentCutsceneType = eCutsceneType.Intro_Refuse;
            CutsceneCheck(currentCutsceneType);
        }
    }

    private void OnIntroAcceptDialogFinished()
    {
        PlayCutscene(currentCutsceneType, 
        () => {
            SceneManager.LoadScene("Game");
        });
    }

    private void OnIntroRefuseDialogFinished()
    {
        isBlocking = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEndingDeathDialogFinished()
    {
        isBlocking = false;
        if (ftuePlayerChoiceMade)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnEndingExplodedDialogFinished()
    {
        isBlocking = false;
        if (ftuePlayerChoiceMade)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnEndingWinDialogFinished()
    {
        PlayCutscene(currentCutsceneType, 
        () => {
            SceneManager.LoadScene("MainMenu");
        });
    }

    private void PlayCutscene(eCutsceneType cutsceneType, Action onCutsceneFinished = null)
    {
        isBlocking = true;
        cutsceneScript.PlayCutscene(cutsceneType, 
        () => {
            onCutsceneFinished?.Invoke();
        });
    }

    //TEXT
    public void PlayText(TMP_Text textBox, string message, float speed = 0.02f)
    {
        StopAllCoroutines();
        isTyping = true;
        typeTextCoroutine = StartCoroutine(TypeText(textBox, message, speed));
    }

    private IEnumerator TypeText(TMP_Text textBox, string message, float speed = 0.02f)
    {
        textBox.text = message;
        textBox.maxVisibleCharacters = 0;

        for (int i = 0; i <= message.Length; i++)
        {
            textBox.maxVisibleCharacters = i;
            yield return new WaitForSeconds(speed);
        }
        isTyping = false;
        typeTextCoroutine = null;
    }
}
