using PlayerModes;
using UnityEngine;

namespace GameStates
{
	public class GameplayState : GameState
	{
		private PlayerMode _currentPlayerMode;
		private NormalMode _normalMode;
		private FocusMode _focusMode;

		private Player _player;

		public GameplayState(GameManager gameManager, Player player) : base(gameManager)
		{
			_player = player;
			_normalMode = new NormalMode(_player);
			_focusMode = new FocusMode(_player);
			SwitchPlayerMode(_normalMode);
		}

		public override void Enter()
		{
		}

		public override void Exit()
		{
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
