using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Alchemy.Serialization;

[AlchemySerialize]
public partial class FeedbackController_FocusState : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [AlchemySerializeField, NonSerialized] private Dictionary<string, MMF_Player> _focusStateFeedbacks = new();

    void Start()
    {
        MMEventManager.AddListener<MMGameEvent>(this);
    }
    void OnDestroy()
    {
        MMEventManager.RemoveListener<MMGameEvent>(this);           
    }
    public void OnFocusStateChange(string focusState)
    {
        // Debug.Log($"Focus state changed: {focusState}");
        // Here you can trigger visual or audio feedback based on the focus state
        // For example, you could play a sound or spawn a particle effect
        if (_focusStateFeedbacks.TryGetValue(focusState, out MMF_Player feedback))
        {
            feedback?.PlayFeedbacks();
        }
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        if (eventType.EventName == GameDefine.FocusModeEvents.EventName)
        {
            OnFocusStateChange(eventType.StringParameter);
        }
    }
}
