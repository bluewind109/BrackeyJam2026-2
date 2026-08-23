using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] private string _spellName;
	[SerializeField] private InputSequence _inputSequence;

	public string SpellName => _spellName;
	public InputSequence InputSequence => _inputSequence;

	public bool IsInputMatched(InputDirection playerInput, int index)
	{
		if (_inputSequence == null) return false;
		return _inputSequence.IsInputMatched(playerInput, index);
	}
}
