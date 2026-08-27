using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public event Action OnPlayerDeath;
	public event Action OnRightMouseClicked;
	public event Action OnSpellTyped;

	[SerializeField] private PlayerStats _playerStats;
	[SerializeField] private PlayerInputHandler _inputHandler;
	[SerializeField] private PlayerTypedInput _typedInput;
	[SerializeField] private List<Spell> _spells = new List<Spell>();

	private Health _health;
	private Hurtbox _hurtbox;

	private int _inputIndex = 0;
	private Spell _currentSpell = null;
	private Spell _spellToType = null;
	private List<InputDirection> _playerInputs = new List<InputDirection>();
	private bool _isFocusModeActive = false;

	void Awake()
	{
		_health = GetComponentInChildren<Health>();
		_hurtbox = GetComponentInChildren<Hurtbox>();

		if (_health != null)
		{
			_health.Initialize(_playerStats.MaxHealth);
			_health.onHealthChanged += OnHealthChanged;
			_health.onDeath += OnDeath;
		}

		if (_hurtbox != null)
		{
			_hurtbox.Initialize(_health);
		}
	}

	private void OnDeath()
	{
		Debug.Log("<color=red>Player has died!</color>");
		OnPlayerDeath?.Invoke();
	}

	private void OnHealthChanged(int value)
	{

	}

	void Start()
	{
		InitInput();
	}

	private void InitInput()
	{
		if (!_inputHandler) return;
		_inputHandler.EnableInput();
		_inputHandler.MouseClicked += OnMouseClicked;
		_inputHandler.InputReceived += OnInputReceived;
	}

	void OnDestroy()
	{
		DisposeInput();
	}

	private void DisposeInput()
	{
		if (!_inputHandler) return;
		_inputHandler.MouseClicked -= OnMouseClicked;
		_inputHandler.InputReceived -= OnInputReceived;
	}

	public void GameUpdate()
	{
		_typedInput.UpdateTypedInput(_playerInputs);

		if (_inputHandler)
		{
			_inputHandler.UpdateMouseInput();

			if (_isFocusModeActive)
			{
				_inputHandler.UpdateKeyboardInput();
			}
			else
			{
				HandleNormalModeMovement(_inputHandler.GetMovementInput());
			}
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
		OnRightMouseClicked?.Invoke();
	}

	private Vector3 GetMouseWorldPosition()
	{
		Vector3 mousePosition = Input.mousePosition;
		// Set this to the distance from the camera to the player
		mousePosition.z = Camera.main.nearClipPlane;
		// Debug.Log($"Mouse Position: {mousePosition}, World Position: {Camera.main.ScreenToWorldPoint(mousePosition)}");
		return Camera.main.ScreenToWorldPoint(mousePosition);
	}

	private void OnInputReceived(InputDirection input)
	{
		if (!_isFocusModeActive) return;

		if (_inputIndex == 0)
		{
			_inputIndex++;
			_spellToType = _spells.Find(spell => spell.IsInputMatched(input, 0));
			// Debug.Log($"First input: {input}. Spell to type: {_spellToType?.name ?? "None"}");
			_playerInputs.Add(input);
			return;
		}
		if (_spellToType == null) return;

		if (_spellToType.IsInputMatched(input, _inputIndex))
		{
			_inputIndex++;
			_playerInputs.Add(input);
			// Debug.Log($"Correct input: {input}. Current sequence: {string.Join(", ", _playerInputs)}");
			bool isSequenceComplete = _inputIndex >= _spellToType.InputSequence.Length;
			if (isSequenceComplete)
			{
				// Debug.Log("<color=green>Input sequence completed!</color>");
				_inputIndex = 0;
				SaveTypedSpell(_spellToType);
				_playerInputs.Clear();
				OnSpellTyped?.Invoke();
			}
		}
		else
		{
			// Debug.Log("Incorrect input. Resetting sequence.");
			_inputIndex = 0;
			_spellToType = null;
			_playerInputs.Clear();
		}
	}

	private void SaveTypedSpell(Spell spell)
	{
		_currentSpell = spell;
	}

	private void HandleNormalModeMovement(Vector2 movementInput)
	{
		if (movementInput == Vector2.zero) return;

		Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f);
		transform.position += movement * _playerStats.MoveSpeed * Time.deltaTime;
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
