using UnityEngine;

public abstract class Spell : ScriptableObject
{
	[Header("Spell Settings")]
    [SerializeField] private string _spellName;
	[SerializeField] private Sprite _spellIcon;
	[SerializeField] private int _damage;
	[SerializeField] private float _castRange;
	[SerializeField] private float _cooldown;
	[SerializeField] private InputSequence _inputSequence;

	public string SpellName => _spellName;
	public Sprite SpellIcon => _spellIcon;
	public int Damage => _damage;
	public float CastRange => _castRange;
	public float Cooldown => _cooldown;
	public InputSequence InputSequence => _inputSequence;

	public bool IsInputMatched(InputDirection playerInput, int index)
	{
		if (_inputSequence == null) return false;
		return _inputSequence.IsInputMatched(playerInput, index);
	}

	public abstract void Cast(Vector3 from, Vector3 to);
}
