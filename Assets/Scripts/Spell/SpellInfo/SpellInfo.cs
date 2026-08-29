using System.Collections.Generic;
using UnityEngine;

public abstract class SpellInfo : ScriptableObject
{
	[Header("Spell Settings")]
	[SerializeField] private SpellType _spellType;
    [SerializeField] private string _spellName;
	[SerializeField] private Sprite _spellIcon;
	[SerializeField] private int _level;
	[SerializeField] private int _damage;
	[SerializeField] private float _cooldown;
	[SerializeField] private InputSequence _inputSequence;

	[Header("Spell Level Info")]
	[SerializeField] private List<SpellLevelInfo> _levelInfos;
	public List<SpellLevelInfo> LevelInfos => _levelInfos;

	public SpellType SpellType => _spellType;
	public string SpellName => _spellName;
	public Sprite SpellIcon => _spellIcon;
	public int Level => _level;
	public int Damage => _damage;
	public float Cooldown => _cooldown;
	public InputSequence InputSequence => _inputSequence;

	public bool IsInputMatched(InputDirection playerInput, int index)
	{
		if (_inputSequence == null) return false;
		return _inputSequence.IsInputMatched(playerInput, index);
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

	public abstract void Cast(Player player, Vector3 to);
}

[System.Serializable]
public class SpellLevelInfo
{
	[SerializeField] private int _damage;
	[SerializeField] private float _cooldown;
	[SerializeField] private int _experienceRequired;
	[SerializeField] private InputSequence _inputSequence;
	private int _currentExperience = 0;

	public int Damage => _damage;
	public float Cooldown => _cooldown;
	public int ExperienceRequired => _experienceRequired;
	public InputSequence InputSequence => _inputSequence;
}

public enum SpellType
{
	FireBall,
	IceLances,
	WindStep
}
