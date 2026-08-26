using UnityEngine;
using GameStates;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private Enemy _enemy;
	[SerializeField] private Timer _focusTimer;

	private GameState _currentState;
	public GameState InitState { get; private set; }
	public GameState IntroState { get; private set; }
	public GameState GameplayState { get; private set; }
	public GameState PauseState { get; private set; }
	public GameState GameOverState { get; private set; }

	void Start()
	{
		InitState = new InitState(this);
		IntroState = new IntroState(this);
		GameplayState = new GameplayState(this, _player, _enemy, _focusTimer);
		PauseState = new PauseState(this);
		GameOverState = new GameOverState(this);

		SetState(InitState);
	}

	void Update()
	{
		_currentState?.Update();
	}

	public void SetState(GameState newState)
	{
		if (newState == null) return;
		if (_currentState == newState) return;

		_currentState?.Exit();
		_currentState = newState;
		_currentState?.Enter();
	}
}
