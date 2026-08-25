using System;
using UnityEngine;

namespace PlayerModes
{
	public class FocusMode : PlayerMode
	{
		public event Action FocusModeComplete;

		private float _focusDuration = 3f;
		private Timer _focusTimer;

		public FocusMode(Player player, Timer timer) : base(player, timer)
		{
			_focusTimer = timer;
		}

		public override void Enter()
		{
			_focusTimer.Begin(_focusDuration);
			_focusTimer.onTimerComplete += OnFocusTimerComplete;
		}

		public override void Exit()
		{
			_focusTimer.onTimerComplete -= OnFocusTimerComplete;
		}

		public override void Update()
		{
			_player.GameUpdate();
			_focusTimer.UpdateTime();
		}

		private void OnFocusTimerComplete()
		{
			FocusModeComplete?.Invoke();
		}
	}
}
