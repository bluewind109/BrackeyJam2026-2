using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDictionary", menuName = "Spell Dictionary")]
public class SpellDictionary : ScriptableObject
{
    [SerializeField] private List<SpellInfo> _spells = new List<SpellInfo>();

	public SpellInfo GetSpellByType(SpellType spellType)
	{
		foreach (var spell in _spells)
		{
			if (spell.SpellType == spellType)
			{
				return spell;
			}
		}
		Debug.LogWarning($"No spell found for type {spellType}.");
		return null;
	}

	public SpellInfo GetSpellByFirstInput(InputDirection input)
	{
		foreach (var spell in _spells)
		{
			if (spell.IsInputMatched(input, 1, 0))
			{
				return spell;
			}
		}
		Debug.LogWarning($"No spell found for input {input}.");
		return null;
	}
}
