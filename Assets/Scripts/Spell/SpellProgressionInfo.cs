using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellProgressionInfo
{
	public event System.Action<SpellProgressionInfo> OnLevelUp;

	private SpellType _spellType;
	private int _level;
	private int _maxLevel;
	private int _experience = 0;
	private int _experienceRequired;

	private float _cooldown;
	private float _cooldownTimer = 0f;

	private Spell_UIElement _uiElement;

	public SpellProgressionInfo(
		SpellType spellType,
		int level,
		int maxLevel,
		int experienceRequired,
		float cooldown,
		Spell_UIElement uiElement)
	{
		_spellType = spellType;
		_level = level;
		_maxLevel = maxLevel;
		_experience = 0;
		_experienceRequired = experienceRequired;
		_cooldown = cooldown;
		_uiElement = uiElement;
	}

	public void OnCasted()
	{
		_cooldownTimer = _cooldown;
		AddExperience();
	}

	private void AddExperience()
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
		_experience = 0;
	}

	public bool IsMaxLevel()
	{
		return _level >= _maxLevel;
	}

	public void UpdateUI(List<InputDirection> inputDirections)
	{
		_uiElement.Initialize(_level, inputDirections);
	}

	public void UpdateExperienceRequired(int newExperienceRequired)
	{
		_experienceRequired = newExperienceRequired;
	}

	public void GameUpdate()
	{
		_cooldownTimer -= Time.deltaTime;
		if (_cooldownTimer < 0f)
		{
			_cooldownTimer = 0f;
		}
		_uiElement.UpdateCooldown(_cooldown, _cooldownTimer);
		_uiElement.UpdateExperience(_experience, _experienceRequired);
	}

	public bool IsOnCooldown()
	{
		return _cooldownTimer > 0f;
	}

	public SpellType SpellType => _spellType;
	public int Level => _level;
	public int Experience => _experience;
	public int ExperienceRequired => _experienceRequired;
}