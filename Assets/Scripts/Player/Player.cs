using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public event Action OnPlayerDeath;
	public event Action OnRightMouseClicked;
	public event Action OnSpellTyped;

	[SerializeField] private PlayerDisplay _playerDisplay;
	[SerializeField] private PlayerStats _playerStats;
	[SerializeField] private PlayerInputHandler _inputHandler;
	[SerializeField] private PlayerTypedInput _typedInput;

	private Health _health;
	private Hurtbox _hurtbox;

	private int _inputIndex = 0;
	private SpellInfo _currentSpell = null;
	private SpellInfo _spellToType = null;
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
		ClampInsideScreen();
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
		SpellProgressionInfo spellProgressionInfo = SpellManager.Instance.GetSpellProgressionInfo(_currentSpell.SpellType);
		int currentLevel = spellProgressionInfo.Level;
		_currentSpell.Cast(currentLevel, this, targetPosition);
		spellProgressionInfo.OnCasted();
		_currentSpell = null; // Reset the current spell after casting

		_playerDisplay.PlayCastSpellAnimation();
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
			_spellToType = SpellManager.Instance.GetSpellByFirstInput(input);
			// Debug.Log($"First input: {input}. Spell to type: {_spellToType?.name ?? "None"}");
			if (_spellToType == null)
			{
				Debug.LogWarning($"No spell found for input {input}. Resetting input sequence.");
				_inputIndex = 0;
				return;
			}

			bool isOnCooldown = SpellManager.Instance.GetSpellProgressionInfo(_spellToType.SpellType).IsOnCooldown();
			if (isOnCooldown)
			{
				Debug.LogWarning($"Spell {_spellToType.name} is on cooldown. Resetting input sequence.");
				_inputIndex = 0;
				return;
			}

			_inputIndex++;
			SavePlayerInput(input);
			return;
		}
		if (_spellToType == null) return;

		int currentLevel = SpellManager.Instance.GetSpellProgressionInfo(_spellToType.SpellType).Level;
		if (_spellToType.IsInputMatched(input, currentLevel, _inputIndex))
		{
			_inputIndex++;
			SavePlayerInput(input);
			// Debug.Log($"Correct input: {input}. Current sequence: {string.Join(", ", _playerInputs)}");
			bool isSequenceComplete = _inputIndex >= _spellToType.GetLevelInfo(currentLevel).InputSequence.Length;
			if (isSequenceComplete)
			{
				// Debug.Log("<color=green>Input sequence completed!</color>");
				_inputIndex = 0;
				SaveTypedSpell(_spellToType);
				ClearInputSequence();
				OnSpellTyped?.Invoke();

				_playerDisplay.TriggerFocusVFXAnimation(_spellToType);
			}
		}
		else
		{
			// Debug.Log("Incorrect input. Resetting sequence.");
			_inputIndex = 0;
			_spellToType = null;
			ClearInputSequence();
		}
	}

	private void SavePlayerInput(InputDirection input)
	{
		_playerInputs.Add(input);
		FocusMode_UI.UpdateSpellToType?.Invoke(_spellToType.SpellType, _playerInputs);
	}

	public void ClearInputSequence()
	{
		_playerInputs.Clear();
		FocusMode_UI.ResetSpellToType?.Invoke();
	}

	private void SaveTypedSpell(SpellInfo spell)
	{
		_currentSpell = spell;
	}

	private void HandleNormalModeMovement(Vector2 movementInput)
	{
		_playerDisplay.UpdateMoving(movementInput);
		if (movementInput == Vector2.zero) return;

		Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f);
		Move(movement * _playerStats.MoveSpeed * Time.deltaTime);
	}

	public void Move(Vector3 movement)
	{
		SetPosition(transform.position + movement);
	}

	public void SetPosition(Vector3 targetPosition)
	{
		transform.position = ScreenBoundsUtility.ClampPositionInsideCamera(Camera.main, transform, targetPosition);
	}

	public void ClampInsideScreen()
	{
		SetPosition(transform.position);
	}

	public void EnterFocusMode()
	{
		_isFocusModeActive = true;
	}

	public void ExitFocusMode()
	{
		_isFocusModeActive = false;
		_inputIndex = 0;
		ClearInputSequence();
	}
}
