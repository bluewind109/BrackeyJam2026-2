using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Alchemy.Serialization;

[AlchemySerialize]
public partial class FeedbackController_Spellcast : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [AlchemySerializeField, NonSerialized] private Dictionary<string, MMF_Player> _spellFeedbacks = new();

    void Start()
    {
        MMEventManager.AddListener<MMGameEvent>(this);
    }
    void OnDestroy()
    {
        MMEventManager.RemoveListener<MMGameEvent>(this);           
    }
    public void OnSpellCast(string spellName)
    {
        // Debug.Log($"Spell cast: {spellName}");
        // Here you can trigger visual or audio feedback based on the spellName
        // For example, you could play a sound or spawn a particle effect
        if (_spellFeedbacks.TryGetValue(spellName, out MMF_Player feedback))
        {
            feedback?.PlayFeedbacks();
        }
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        if (eventType.EventName == GameDefine.SpellEvents.OnSpellCast)
        {
            OnSpellCast(eventType.StringParameter);
        }
    }
}
