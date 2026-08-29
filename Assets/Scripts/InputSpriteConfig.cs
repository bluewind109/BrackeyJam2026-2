using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSpriteConfig", menuName = "Input Sprite Config")]
public class InputSpriteConfig : ScriptableObject
{
	[SerializeField] private List<InputSpriteInfo> _inputSprites = new List<InputSpriteInfo>();
	[SerializeField] private List<InputSpriteInfo> _inputSprites_FocusMode = new List<InputSpriteInfo>();
	[SerializeField] private List<InputSpriteInfo> _inputSprites_FocusMode_Typed = new List<InputSpriteInfo>();

	public Sprite GetInputSprite(InputDirection inputDirection)
	{
		foreach (var inputSprite in _inputSprites)
		{
			if (inputSprite.InputDirection == inputDirection)
			{
				return inputSprite.Sprite;
			}
		}
		Debug.LogWarning($"No sprite found for input direction {inputDirection}.");
		return null;
	}

	public Sprite GetInputSprite_FocusMode(InputDirection inputDirection)
	{
		foreach (var inputSprite in _inputSprites_FocusMode)
		{
			if (inputSprite.InputDirection == inputDirection)
			{
				return inputSprite.Sprite;
			}
		}
		Debug.LogWarning($"No sprite found for input direction {inputDirection} in Focus Mode.");
		return null;
	}

	public Sprite GetInputSprite_FocusMode_Typed(InputDirection inputDirection)
	{
		foreach (var inputSprite in _inputSprites_FocusMode_Typed)
		{
			if (inputSprite.InputDirection == inputDirection)
			{
				return inputSprite.Sprite;
			}
		}
		Debug.LogWarning($"No sprite found for input direction {inputDirection} in Focus Mode Typed.");
		return null;
	}
}

[System.Serializable]
public class InputSpriteInfo
{
	public InputDirection InputDirection;
	public Sprite Sprite;
}