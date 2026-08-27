using System;
using UnityEngine;

namespace PlayerModes
{
	public class NormalMode : PlayerMode
	{
		public event Action FocusModeRequested;

		private float _focusCooldown = 1f;
		private Timer _focusCooldownTimer;

		private Enemy _enemy;

		public NormalMode(Player player, Enemy enemy, Timer timer) : base(player, timer)
		{
			_enemy = enemy;
			_focusCooldownTimer = timer;
		}

		public override void Enter()
		{
			_player.OnRightMouseClicked += OnRightMouseClicked;
			_player.ExitFocusMode();

			_focusCooldownTimer.Begin(_focusCooldown, true);
			_focusCooldownTimer.onTimerComplete += OnFocusCooldownTimerComplete;
		}

		public override void Exit()
		{
			_player.OnRightMouseClicked -= OnRightMouseClicked;
			_focusCooldownTimer.onTimerComplete -= OnFocusCooldownTimerComplete;
		}

		public override void Update()
		{
			_player.GameUpdate();
			_enemy.GameUpdate();
			PlayerProjectileManager.Instance.GameUpdate();
			EnemyProjectileManager.Instance.GameUpdate();
			_focusCooldownTimer.UpdateTime();
		}

		private void OnRightMouseClicked()
		{
			Debug.Log("Right mouse clicked in NormalMode");
			if (_focusCooldownTimer.IsRunning) return;
			FocusModeRequested?.Invoke();
		}

		private void OnFocusCooldownTimerComplete()
		{
			
		}
	}
}
