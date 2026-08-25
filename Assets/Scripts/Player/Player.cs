using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public event Action RightMouseClicked;

	[SerializeField] private PlayerInputHandler _inputHandler;
	[SerializeField] private List<Spell> _spells = new List<Spell>();

	private int _inputIndex = 0;
	private Spell _currentSpell = null;
	private Spell _spellToType = null;
	private List<InputDirection> _playerInputs = new List<InputDirection>();
	private bool _isFocusModeActive = false;

	void Start()
	{
		InitInput();
	}

	private void InitInput()
	{
		if (!_inputHandler) return;
		_inputHandler.EnableInput();
		_inputHandler.MouseClicked += OnMouseClicked;
		_inputHandler.LetterTyped += OnLetterTyped;
	}

	void OnDestroy()
	{
		DisposeInput();
	}

	private void DisposeInput()
	{
		if (!_inputHandler) return;
		_inputHandler.MouseClicked -= OnMouseClicked;
		_inputHandler.LetterTyped -= OnLetterTyped;
	}

	public void GameUpdate()
	{
		if (_inputHandler)
		{
			_inputHandler.UpdateMouseInput();
			_inputHandler.UpdateKeyboardInput();
		}
	}

	private void OnMouseClicked(MouseButton button)
	{
		if (button == MouseButton.None) return;

		if (button == MouseButton.Left)
		{
			HandleLeftClick();
		}
		else if (button == MouseButton.Right)
		{
			HandleRightClick();
		}
	}

	private void HandleLeftClick()
	{
		if (_currentSpell == null) return;
		Vector3 targetPosition = GetMouseWorldPosition();
		targetPosition.z = 0f;
		_currentSpell.Cast(this, targetPosition);
		_currentSpell = null; // Reset the current spell after casting
	}

	private void HandleRightClick()
	{
		RightMouseClicked?.Invoke();
	}

	private Vector3 GetMouseWorldPosition()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = Camera.main.nearClipPlane; // Set this to the distance from the camera to the player
													 // Debug.Log($"Mouse Position: {mousePosition}, World Position: {Camera.main.ScreenToWorldPoint(mousePosition)}");
		return Camera.main.ScreenToWorldPoint(mousePosition);
	}

	private void OnLetterTyped(InputDirection input)
	{
		if (_inputIndex == 0)
		{
			_inputIndex++;
			_spellToType = _spells.Find(spell => spell.IsInputMatched(input, 0));
			Debug.Log($"First input: {input}. Spell to type: {_spellToType?.name ?? "None"}");
			return;
		}
		if (_spellToType == null) return;

		if (_spellToType.IsInputMatched(input, _inputIndex))
		{
			_inputIndex++;
			_playerInputs.Add(input);
			Debug.Log($"Correct input: {input}. Current sequence: {string.Join(", ", _playerInputs)}");
			if (_inputIndex >= _spellToType.InputSequence.Length)
			{
				Debug.Log("<color=green>Input sequence completed!</color>");
				_inputIndex = 0; // Reset for next sequence
				_currentSpell = _spellToType; // Set the current spell to the completed spell
				_playerInputs.Clear();
			}
		}
		else
		{
			Debug.Log("Incorrect input. Resetting sequence.");
			_inputIndex = 0; // Reset on incorrect input
			_spellToType = null; // Reset the spell to type
			_playerInputs.Clear();
		}
	}

	public void EnterFocusMode()
	{
		_isFocusModeActive = true;
	}

	public void ExitFocusMode()
	{
		_isFocusModeActive = false;
	}
}
