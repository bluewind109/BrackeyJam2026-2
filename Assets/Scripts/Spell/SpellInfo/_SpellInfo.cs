using System.Collections.Generic;
using UnityEngine;

public abstract class SpellInfo : ScriptableObject
{
	[Header("Spell Settings")]
	[SerializeField] private SpellType _spellType;
	[SerializeField] private Sprite _spellIcon;
	[SerializeField] private string _spellName;

	[Header("Spell Level Info")]
	[SerializeField] private List<SpellLevelInfo> _levelInfos;
	public List<SpellLevelInfo> LevelInfos => _levelInfos;

	public SpellType SpellType => _spellType;
	public Sprite SpellIcon => _spellIcon;
	public string SpellName => _spellName;
	public int MaxLevel => _levelInfos.Count;

	public InputSequence GetInputSequence(int level)
	{
		if (level < 1 || level > _levelInfos.Count)
		{
			Debug.LogWarning($"Invalid level {level} for spell {_spellName}. Returning null.");
			return null;
		}
		return _levelInfos[level - 1].InputSequence;
	}

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

public enum SpellType
{
	FireBall,
	IceLances,
	WindStep
}
