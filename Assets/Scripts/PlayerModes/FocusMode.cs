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
			_player.EnterFocusMode();
			_player.OnSpellTyped += OnSpellTyped;
			
			_focusTimer.Begin(_focusDuration);
			_focusTimer.onTimerComplete += OnFocusTimerComplete;
		}

		public override void Exit()
		{
			_focusTimer.onTimerComplete -= OnFocusTimerComplete;
			_player.OnSpellTyped -= OnSpellTyped;
		}

		public override void Update()
		{
			_player.GameUpdate();
			_focusTimer.UpdateTime();
		}

		private void OnSpellTyped()
		{
			Debug.Log("Spell typed in FocusMode");
			OnFocusTimerComplete();
		}

		private void OnFocusTimerComplete()
		{
			FocusModeComplete?.Invoke();
		}
	}
}
