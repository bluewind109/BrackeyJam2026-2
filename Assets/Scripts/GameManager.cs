using UnityEngine;
using GameStates;

public class GameManager : MonoBehaviour
{
    private GameState _currentState;
    private GameState _introState;
    private GameState _gameplayState;
    private GameState _pauseState;
    private GameState _gameOverState;

    void Start()
    {

    }

    void Update()
    {

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
