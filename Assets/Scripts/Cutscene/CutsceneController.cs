using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;
using System;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private CutsceneScript cutsceneScript;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private CanvasGroup dialogTextCanvasGroup;
    [Header("Dialog Configs")]
    [SerializeField] private CutsceneCollection cutsceneCollection;
    private int currentSentenceIndex = 0;
    DialogConfig currentDialog;
    private SentenceData currentSentenceData;
    private eCutsceneType currentCutsceneType = eCutsceneType.Intro;
    private bool ftuePlayerChoiceMade = true;
    private bool isBlocking = true; // Prevents input during cutscenes or dialogs
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        if (Input.GetMouseButtonDown(0))
        {
            ftuePlayerChoiceMade = true;
            NextSentence();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ftuePlayerChoiceMade = false;
            NextSentence();
        }
    }

    private void CutsceneCheck(eCutsceneType cutsceneType)
    {
        currentDialog = cutsceneCollection.GetDialogConfig(cutsceneType);
        switch (cutsceneType)
        {
            case eCutsceneType.Intro:
                isBlocking = true;
                cutsceneScript.PlayCutscene(cutsceneType, 
                    () => {
                        isBlocking = false;
                        PlayDialog(currentDialog);
                    });
                break;
            case eCutsceneType.Intro_Accept:
                isBlocking = false;
                PlayDialog(currentDialog);
                break;
            case eCutsceneType.Intro_Refuse:
                isBlocking = false;
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
            dialogTextCanvasGroup.DOFade(1, 0.5f);
            currentSentenceIndex = 0;
            currentSentenceData = dialogConfig.SentenceDataList[currentSentenceIndex];
            if (currentSentenceData != null)
            {
                characterNameText.text = currentSentenceData.CharacterName.ToString();
                PlayText(dialogText, currentSentenceData.SentenceText, 0.05f);
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
                PlayText(dialogText, currentSentenceData.SentenceText, 0.05f);
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
        isBlocking = true;
        cutsceneScript.PlayCutscene(currentCutsceneType, 
        () => {
            // Load scene
        });
    }

    private void OnIntroRefuseDialogFinished()
    {
        isBlocking = false;
        cutsceneScript.PlayCutscene(currentCutsceneType, 
        () => {
            // Load scene
        });
    }

    //TEXT
    public void PlayText(TMP_Text textBox, string message, float speed = 0.05f)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(textBox, message, speed));
    }

    private IEnumerator TypeText(TMP_Text textBox, string message, float speed = 0.05f)
    {
        textBox.text = message;
        textBox.maxVisibleCharacters = 0;

        for (int i = 0; i <= message.Length; i++)
        {
            textBox.maxVisibleCharacters = i;
            yield return new WaitForSeconds(speed);
        }
    }
}
