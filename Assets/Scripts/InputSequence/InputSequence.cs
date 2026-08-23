using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InputSequence", menuName = "InputSequence")]
public class InputSequence : ScriptableObject
{
    [SerializeField] private List<InputDirection> _inputs = new List<InputDirection>();

	public List<InputDirection> Inputs => _inputs;
	public int Length => _inputs.Count;

	public InputDirection GetInputAt(int index)
	{
		if (index < 0 || index >= _inputs.Count) return InputDirection.None;
		return _inputs[index];
	}

	public bool IsInputMatched(InputDirection playerInput, int index)
	{
		if (playerInput == InputDirection.None) return false;
		InputDirection input = GetInputAt(index);
		return input == playerInput;
	}
}
