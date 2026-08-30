using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogConfig", menuName = "Narrative/DialogConfig", order = 1)]
public class DialogConfig : ScriptableObject
{
    [SerializeField] private List<SentenceData> sentenceDataList;
    public List<SentenceData> SentenceDataList => sentenceDataList;
}

[Serializable]
public class CutsceneCollection
{
    [SerializeField] private List<CutsceneItem> cutsceneItems;

    public DialogConfig GetDialogConfig(eCutsceneType eCutsceneType)
    {
        foreach (var cutsceneItem in cutsceneItems)
        {
            if (cutsceneItem.CutsceneType == eCutsceneType)
            {
                return cutsceneItem.DialogConfig;
            }
        }
        Debug.LogWarning($"No dialog config found for cutscene type: {eCutsceneType}. Returning null.");
        return null;
    }
}

[Serializable]
public class CutsceneItem
{
    [SerializeField] private eCutsceneType cutsceneType;
    [SerializeField] private DialogConfig dialogConfig;

    public eCutsceneType CutsceneType => cutsceneType;
    public DialogConfig DialogConfig => dialogConfig;
}

[Serializable]
public class SentenceData
{
    [SerializeField] private eTalkingCharacter characterName;
    [SerializeField] private string sentenceText;
    [SerializeField] private string choiceMessage;

    public eTalkingCharacter CharacterName => characterName;
    public string SentenceText => sentenceText;
    public string ChoiceMessage => choiceMessage;
}

public enum eTalkingCharacter
{
    You,
    Orb
}
