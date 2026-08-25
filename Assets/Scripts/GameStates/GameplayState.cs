using PlayerModes;
using UnityEngine;

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

		public GameplayState(GameManager gameManager, Player player, Enemy enemy, Timer timer) : base(gameManager)
		{
			_player = player;
			_enemy = enemy;
			_focusTimer = timer;
			_normalMode = new NormalMode(_player, _enemy, _focusTimer);
			_focusMode = new FocusMode(_player, _focusTimer);
			SwitchPlayerMode(_normalMode);
		}

		public override void Enter()
		{
			_normalMode.FocusModeRequested += () => SwitchPlayerMode(_focusMode);
			_focusMode.FocusModeComplete += () => SwitchPlayerMode(_normalMode);
		}

		public override void Exit()
		{
			_normalMode.FocusModeRequested -= () => SwitchPlayerMode(_focusMode);
			_focusMode.FocusModeComplete -= () => SwitchPlayerMode(_normalMode);
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
	}
}
