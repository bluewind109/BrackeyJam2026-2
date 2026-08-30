using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Alchemy.Serialization;

[AlchemySerialize]
public partial class Orb : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [AlchemySerializeField, NonSerialized] private Dictionary<SpellType, Color> _spellCastFeedbacks = new();
    [SerializeField] private Color _defaultColor = Color.gray;
    [SerializeField] private MMFollowTarget _followTarget;
    [SerializeField] private SpriteRenderer _spriteRendererOutline;

    public void OnMMEvent(MMGameEvent eventType)
    {
        if (eventType.EventName == GameDefine.SpellEvents.OnSpellCast)
        {
            TriggerOutlineFeedback((SpellType)eventType.IntParameter);
        }
        else if (eventType.EventName == GameDefine.SpellEvents.OnSpellFinish)
        {
            TriggerOutlineFeedback(SpellType.None);
        }
    }
    void TriggerOutlineFeedback(SpellType spellName)
    {
        if (_spellCastFeedbacks.TryGetValue(spellName, out Color color))
        {
            _spriteRendererOutline.color = color;
        }
        else
        {
            _spriteRendererOutline.color = _defaultColor;
            Debug.LogWarning($"Spell cast feedback for spell name {spellName} not found.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._followTarget.Target = GameObject.FindGameObjectWithTag("Player").transform;
        MMEventManager.AddListener<MMGameEvent>(this);

        TriggerOutlineFeedback(SpellType.None);
    }
    void OnDestroy()
    {
        MMEventManager.RemoveListener<MMGameEvent>(this);           
    }
}
