using System.Collections.Generic;
using UnityEngine;

public abstract class SpellInfo : ScriptableObject
{
	[Header("Spell Settings")]
	[SerializeField] private SpellType _spellType;
	[SerializeField] private string _spellName;
	[SerializeField] private Sprite _spellIcon;
	[SerializeField] private int _level;

	[Header("Spell Level Info")]
	[SerializeField] private List<SpellLevelInfo> _levelInfos;
	public List<SpellLevelInfo> LevelInfos => _levelInfos;

	public SpellType SpellType => _spellType;
	public string SpellName => _spellName;
	public Sprite SpellIcon => _spellIcon;
	public int Level => _level;
	public int MaxLevel => _levelInfos.Count;

	public bool IsInputMatched(InputDirection playerInput, int level, int index)
	{
		SpellLevelInfo levelInfo = GetLevelInfo(level);
		if (levelInfo == null) return false;
		InputSequence inputSequence = levelInfo.InputSequence;
		if (inputSequence == null) return false;
		return inputSequence.IsInputMatched(playerInput, index);
	}

	public SpellLevelInfo GetLevelInfo(int level)
	{
		if (level < 1 || level > _levelInfos.Count)
		{
			Debug.LogWarning($"Invalid level {level} for spell {_spellName}. Returning null.");
			return null;
		}
		return _levelInfos[level - 1];
	}

	public abstract void Cast(int level, Player player, Vector3 to);
}

[System.Serializable]
public class SpellLevelInfo
{
	[SerializeField] private int _damage;
	[SerializeField] private float _cooldown;
	[SerializeField] private int _experienceRequired;
	[SerializeField] private InputSequence _inputSequence;

	public int Damage => _damage;
	public float Cooldown => _cooldown;
	public int ExperienceRequired => _experienceRequired;
	public InputSequence InputSequence => _inputSequence;
}

[System.Serializable]
public class SpellProgressionInfo
{
	public event System.Action<SpellProgressionInfo> OnLevelUp;
	public event System.Action<SpellType> OnMaxLevelReached;

	private SpellType _spellType;
	private int _level;
	private int _maxLevel;
	private int _experience = 0;
	private int _experienceRequired;

	public SpellProgressionInfo(
		SpellType spellType,
		int level,
		int maxLevel,
		int experienceRequired)
	{
		_spellType = spellType;
		_level = level;
		_maxLevel = maxLevel;
		_experience = 0;
		_experienceRequired = experienceRequired;
	}

	public void AddExperience()
	{
		_experience++;
		if (_experience >= _experienceRequired)
		{
			Debug.Log($"Spell {_spellType} leveled up to level {_level}!");
			OnLevelUp?.Invoke(this);
		}
	}

	public void LevelUp()
	{
		_level++;

		if (_level > _maxLevel)
		{
			OnMaxLevelReached?.Invoke(_spellType);
			return;
		}

		_experience = 0;
	}

	public void UpdateExperienceRequired(int newExperienceRequired)
	{
		_experienceRequired = newExperienceRequired;
	}

	public SpellType SpellType => _spellType;
	public int Level => _level;
	public int Experience => _experience;
	public int ExperienceRequired => _experienceRequired;
}

public enum SpellType
{
	FireBall,
	IceLances,
	WindStep
}
