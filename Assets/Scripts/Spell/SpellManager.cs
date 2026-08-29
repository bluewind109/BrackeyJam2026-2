using UnityEngine;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance { get; private set; }

    [SerializeField] private SpellDictionary _spellDictionary;

    [SerializeField] private Spell_UIElement _fireBall_UIElement;
    [SerializeField] private Spell_UIElement _iceLances_UIElement;
    [SerializeField] private Spell_UIElement _windStep_UIElement;

    Dictionary<SpellType, SpellProgressionInfo> _spellProgressionInfos = new Dictionary<SpellType, SpellProgressionInfo>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Initialize()
    {
        int startLevel = 1;
        InitFireBallSpellProgression(startLevel);
        InitIceLancesSpellProgression(startLevel);
        InitWindStepSpellProgression(startLevel);
    }

    private void InitFireBallSpellProgression(int startLevel)
    {
        SpellInfo fireBallSpellInfo = _spellDictionary.GetSpellByType(SpellType.FireBall);
        SpellLevelInfo fireBallLevelInfo = fireBallSpellInfo.GetLevelInfo(startLevel);
        SpellProgressionInfo fireBallProgress = new SpellProgressionInfo(
            SpellType.FireBall,
            startLevel,
            fireBallSpellInfo.MaxLevel,
            fireBallLevelInfo.ExperienceRequired,
            fireBallLevelInfo.Cooldown,
            _fireBall_UIElement
        );
        _spellProgressionInfos.Add(SpellType.FireBall, fireBallProgress);
		_fireBall_UIElement.Initialize(startLevel, fireBallLevelInfo.InputSequence.GetInputs());
		fireBallProgress.OnLevelUp += OnSpellLevelUp;
    }

    private void InitIceLancesSpellProgression(int startLevel)
    {
        SpellInfo iceLancesSpellInfo = _spellDictionary.GetSpellByType(SpellType.IceLances);
        SpellLevelInfo iceLancesLevelInfo = iceLancesSpellInfo.GetLevelInfo(startLevel);
        SpellProgressionInfo iceLancesProgress = new SpellProgressionInfo(
            SpellType.IceLances,
            startLevel,
            iceLancesSpellInfo.MaxLevel,
            iceLancesLevelInfo.ExperienceRequired,
            iceLancesLevelInfo.Cooldown,
            _iceLances_UIElement
        );
        _spellProgressionInfos.Add(SpellType.IceLances, iceLancesProgress);
        _iceLances_UIElement.Initialize(startLevel, iceLancesLevelInfo.InputSequence.GetInputs());
        iceLancesProgress.OnLevelUp += OnSpellLevelUp;
    }

    private void InitWindStepSpellProgression(int startLevel)
    {
        SpellInfo windStepSpellInfo = _spellDictionary.GetSpellByType(SpellType.WindStep);
        SpellLevelInfo windStepLevelInfo = windStepSpellInfo.GetLevelInfo(startLevel);
        SpellProgressionInfo windStepProgress = new SpellProgressionInfo(
            SpellType.WindStep,
            startLevel,
            windStepSpellInfo.MaxLevel,
            windStepLevelInfo.ExperienceRequired,
            windStepLevelInfo.Cooldown,
            _windStep_UIElement
        );
        _spellProgressionInfos.Add(SpellType.WindStep, windStepProgress);
        _windStep_UIElement.Initialize(startLevel, windStepLevelInfo.InputSequence.GetInputs());
        windStepProgress.OnLevelUp += OnSpellLevelUp;
    }

    private void OnSpellLevelUp(SpellProgressionInfo spellProgressionInfo)
    {
        Debug.Log($"Spell {spellProgressionInfo.SpellType} leveled up!");
        SpellType spellType = spellProgressionInfo.SpellType;
        SpellInfo spellInfo = _spellDictionary.GetSpellByType(spellType);
        spellProgressionInfo.LevelUp();
        if (spellProgressionInfo.IsMaxLevel())
        {
            OnSpellMaxLevelReached();
            return;
        }

        int newExperienceRequired = spellInfo.GetLevelInfo(spellProgressionInfo.Level).ExperienceRequired;
        spellProgressionInfo.UpdateExperienceRequired(newExperienceRequired);

        List<InputDirection> inputDirections = spellInfo.GetLevelInfo(spellProgressionInfo.Level).InputSequence.GetInputs();
        spellProgressionInfo.UpdateUI(inputDirections);
    }

    private void OnSpellMaxLevelReached()
    {
        // TODO player die => game over
    }

    public SpellInfo GetSpellByFirstInput(InputDirection input)
    {
        return _spellDictionary.GetSpellByFirstInput(input);
    }

    public SpellProgressionInfo GetSpellProgressionInfo(SpellType spellType)
    {
        if (_spellProgressionInfos.TryGetValue(spellType, out SpellProgressionInfo progressionInfo))
        {
            return progressionInfo;
        }
        Debug.LogWarning($"No progression info found for spell type {spellType}.");
        return null;
    }

    public void GameUpdate()
    {
        foreach (var spellProgressionInfo in _spellProgressionInfos.Values)
        {
            spellProgressionInfo.GameUpdate();
        }
    }
}
