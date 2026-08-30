using PlayerModes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStates
{
	public class GameplayState : GameState
	{

		private PlayerMode _currentPlayerMode;
		private NormalMode _normalMode;
		private FocusMode _focusMode;

		private Timer _focusTimer;

		private Player _player;
		private Enemy _enemy;
		private FocusMode_UI _focusModeUI;

		public GameplayState(GameManager gameManager, Player player, Enemy enemy, FocusMode_UI focusModeUI, Timer timer) : base(gameManager)
		{
			_player = player;
			_enemy = enemy;
			_focusTimer = timer;
			_focusModeUI = focusModeUI;

			_normalMode = new NormalMode(_player, _enemy, _focusTimer);
			_focusMode = new FocusMode(_player, _focusModeUI, _focusTimer);
			SwitchPlayerMode(_normalMode);
		}

		public override void Enter()
		{
			_player.OnPlayerDeath += OnPlayerDeath;
			_normalMode.FocusModeRequested += () => SwitchPlayerMode(_focusMode);
			_focusMode.FocusModeComplete += () => SwitchPlayerMode(_normalMode);

			SpellManager.Instance.OnMaxLevelReached += OnPlayerDeath;
			_enemy.OnEnemyDeath += OnEnemyDeath;
		}

		public override void Exit()
		{
			_player.OnPlayerDeath -= OnPlayerDeath;
			_normalMode.FocusModeRequested -= () => SwitchPlayerMode(_focusMode);
			_focusMode.FocusModeComplete -= () => SwitchPlayerMode(_normalMode);

			SpellManager.Instance.OnMaxLevelReached -= OnPlayerDeath;
			_enemy.OnEnemyDeath -= OnEnemyDeath;
		}

		public override void Update()
		{
			_currentPlayerMode?.Update();
		}

		private void SwitchPlayerMode(PlayerMode newMode)
		{
			if (newMode == null) return;
			if (newMode == _currentPlayerMode) return;

			_currentPlayerMode?.Exit();
			_currentPlayerMode = newMode;
			_currentPlayerMode?.Enter();
		}

		private void OnPlayerDeath()
		{
			Debug.Log("<color=red>Player has died! Game Over!</color>");
			_gameManager.SetState(_gameManager.GameOverState);
		}

		private void OnEnemyDeath()
		{
			Debug.Log("<color=green>Enemy has died! You Win!</color>");
			_gameManager.SetState(_gameManager.GameOverState);
		}
	}
}
