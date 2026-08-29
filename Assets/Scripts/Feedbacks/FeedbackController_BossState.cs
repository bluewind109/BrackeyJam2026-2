using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Alchemy.Serialization;

[AlchemySerialize]
public partial class FeedbackController_BossState : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [AlchemySerializeField, NonSerialized] private Dictionary<string, MMF_Player> _bossStateFeedbacks = new();

    void Start()
    {
        MMEventManager.AddListener<MMGameEvent>(this);
    }
    void OnDestroy()
    {
        MMEventManager.RemoveListener<MMGameEvent>(this);           
    }
    public void OnBossStateChange(string bossState)
    {
        // Debug.Log($"Boss state changed: {bossState}");
        // Here you can trigger visual or audio feedback based on the boss state
        // For example, you could play a sound or spawn a particle effect
        if (_bossStateFeedbacks.TryGetValue(bossState, out MMF_Player feedback))
        {
            feedback?.PlayFeedbacks();
        }
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        if (eventType.EventName == GameDefine.BossEvents.EventName)
        {
            OnBossStateChange(eventType.StringParameter);
        }
    }
}
