using UnityEngine;
using UnityEngine.Playables;
using System;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private CutsceneData[] cutsceneDataArray;
    private bool isCutscenePlaying = false;
    private eCutsceneType currentCutsceneType = eCutsceneType.Intro;
    private Action OnCutsceneFinished;

    public void PlayCutscene(eCutsceneType cutsceneType, Action onCutsceneFinished = null)
    {
        if (isCutscenePlaying)
        {
            Debug.LogWarning("A cutscene is already playing. Cannot play another cutscene.");
            return;
        }

        currentCutsceneType = cutsceneType;
        director.playableAsset = GetCutscenePlayableAsset(currentCutsceneType);
        director.Play();
        OnCutsceneFinished = onCutsceneFinished;
        director.stopped += OnCutsceneStopped;
        isCutscenePlaying = true;
    }

    private PlayableAsset GetCutscenePlayableAsset(eCutsceneType cutsceneType)
    {
        // Implement your logic to return the correct PlayableAsset based on the cutsceneType
        // For example, you might have a list of CutsceneData and find the matching one
        foreach (var cutsceneData in cutsceneDataArray)
        {
            if (cutsceneData.cutsceneType == cutsceneType)
            {
                return cutsceneData.playableAsset;
            }
        }
        return cutsceneDataArray[0].playableAsset; // Default to the first cutscene if not found
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        isCutscenePlaying = false;
        OnCutsceneFinished?.Invoke();
        director.stopped -= OnCutsceneStopped;
    }
}

public enum eCutsceneType
{
    Intro = 0, // Giới thiệu game
    Intro_Accept, // Chọn trợ giúp
    Intro_Refuse, // Chọn từ chối

    Ending_Death, // Bị giết
    Ending_Exploded, // Bị nổ
    Ending_Win, // Thắng game
}

[System.Serializable]
public class CutsceneData
{
    public eCutsceneType cutsceneType;
    public PlayableAsset playableAsset;
}
