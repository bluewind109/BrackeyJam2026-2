using UnityEngine;
using GameStates;

public class GameManager : MonoBehaviour
{
  [SerializeField] private Player _player;
  [SerializeField] private Timer _focusTimer;

  private GameState _currentState;
  private GameState _introState;
  private GameState _gameplayState;
  private GameState _pauseState;
  private GameState _gameOverState;


  void Start()
  {
    _introState = new IntroState(this);
    _gameplayState = new GameplayState(this, _player, _focusTimer);
    _pauseState = new PauseState(this);
    _gameOverState = new GameOverState(this);

    SetState(_gameplayState);
  }

  void Update()
  {
    _currentState?.Update();
  }

  private void SetState(GameState newState)
  {
    if (newState == null) return;
    if (_currentState == newState) return;

    _currentState?.Exit();
    _currentState = newState;
    _currentState?.Enter();
  }
}
