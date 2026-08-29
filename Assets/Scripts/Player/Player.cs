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
	[SerializeField] private SpellDictionary _spellDictionary;

	Dictionary<SpellType, SpellProgressionInfo> _spellProgressionInfos = new Dictionary<SpellType, SpellProgressionInfo>();

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

		InitializeSpellProgression();

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

	private void InitializeSpellProgression()
	{
		int startLevel = 1;

		SpellInfo fireBallSpellInfo = _spellDictionary.GetSpellByType(SpellType.FireBall);
		SpellLevelInfo fireBallLevelInfo = fireBallSpellInfo.GetLevelInfo(startLevel);
		SpellProgressionInfo fireBallProgress = new SpellProgressionInfo(
			SpellType.FireBall, 
			startLevel, 
			fireBallSpellInfo.MaxLevel, 
			fireBallLevelInfo.ExperienceRequired
		);
		_spellProgressionInfos[SpellType.FireBall] = fireBallProgress;
		fireBallProgress.OnLevelUp += OnSpellLevelUp;
		fireBallProgress.OnMaxLevelReached += OnSpellMaxLevelReached;

		SpellInfo iceLancesSpellInfo = _spellDictionary.GetSpellByType(SpellType.IceLances);
		SpellLevelInfo iceLancesLevelInfo = iceLancesSpellInfo.GetLevelInfo(startLevel);
		SpellProgressionInfo iceLancesProgress = new SpellProgressionInfo(
			SpellType.IceLances, 
			startLevel, 
			iceLancesSpellInfo.MaxLevel, 
			iceLancesLevelInfo.ExperienceRequired
		);
		_spellProgressionInfos[SpellType.IceLances] = iceLancesProgress;
		iceLancesProgress.OnLevelUp += OnSpellLevelUp;
		iceLancesProgress.OnMaxLevelReached += OnSpellMaxLevelReached;

		SpellInfo windStepSpellInfo = _spellDictionary.GetSpellByType(SpellType.WindStep);
		SpellLevelInfo windStepLevelInfo = windStepSpellInfo.GetLevelInfo(startLevel);
		SpellProgressionInfo windStepProgress = new SpellProgressionInfo(
			SpellType.WindStep, 
			startLevel, 
			windStepSpellInfo.MaxLevel, 
			windStepLevelInfo.ExperienceRequired
		);
		_spellProgressionInfos[SpellType.WindStep] = windStepProgress;
		windStepProgress.OnLevelUp += OnSpellLevelUp;
		windStepProgress.OnMaxLevelReached += OnSpellMaxLevelReached;
	}

	private void OnSpellLevelUp(SpellProgressionInfo spellProgressionInfo)
	{
		Debug.Log($"Spell {spellProgressionInfo.SpellType} leveled up!");
		spellProgressionInfo.LevelUp();

		// TODO update spell UI
	}

	private void OnSpellMaxLevelReached(SpellType spellType)
	{
		Debug.Log($"Spell {spellType} reached max level!");

		// TODO player die => game over
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
		int currentLevel = _spellProgressionInfos[_currentSpell.SpellType].Level;
		_currentSpell.Cast(currentLevel, this, targetPosition);
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
			_spellToType = _spellDictionary.GetSpellByFirstInput(input);
			// Debug.Log($"First input: {input}. Spell to type: {_spellToType?.name ?? "None"}");
			_playerInputs.Add(input);
			return;
		}
		if (_spellToType == null) return;

		int currentLevel = _spellProgressionInfos[_spellToType.SpellType].Level;
		if (_spellToType.IsInputMatched(input, currentLevel, _inputIndex))
		{
			_inputIndex++;
			_playerInputs.Add(input);
			// Debug.Log($"Correct input: {input}. Current sequence: {string.Join(", ", _playerInputs)}");
			bool isSequenceComplete = _inputIndex >= _spellToType.GetLevelInfo(currentLevel).InputSequence.Length;
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

	public void ClearInputSequence()
	{
		_inputIndex = 0;
		_playerInputs.Clear();
	}

	private void SaveTypedSpell(SpellInfo spell)
	{
		_currentSpell = spell;
	}

	private void HandleNormalModeMovement(Vector2 movementInput)
	{
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
		ClearInputSequence();
	}
}
