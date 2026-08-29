using UnityEngine;

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
