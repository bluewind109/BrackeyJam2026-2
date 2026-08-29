using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
namespace PlayerModes
{
	public class FocusMode : PlayerMode
	{
		public event Action FocusModeComplete;

		private FocusMode_UI _focusModeUI;
		private float _focusDuration = 3f;
		private Timer _focusTimer;

		public FocusMode(Player player, FocusMode_UI focusModeUI, Timer timer) : base(player, timer)
		{
			_focusModeUI = focusModeUI;
			_focusTimer = timer;
		}

		public override void Enter()
		{
			_player.EnterFocusMode();
			_player.OnSpellTyped += OnSpellTyped;
			
			List<SpellToType_UI_Data> spellDatas = GenerateSpellDatas();
			_focusModeUI.Show(spellDatas);
			_focusTimer.Begin(_focusDuration);
			_focusTimer.onTimerComplete += OnFocusTimerComplete;
		}

		public override void Exit()
		{
			_focusModeUI.Hide();
			_focusTimer.onTimerComplete -= OnFocusTimerComplete;
			_player.OnSpellTyped -= OnSpellTyped;
		}

		public override void Update()
		{
			_player.GameUpdate();
			_focusTimer.UpdateTime();
			_focusModeUI.UpdateFocusBar(_focusTimer.GetRemainingTime() / _focusDuration);
		}

		private List<SpellToType_UI_Data> GenerateSpellDatas()
		{
			List<SpellToType_UI_Data> spellDatas = SpellManager.Instance.GetSpellDataForUI();
			return spellDatas;
		}

		private void OnSpellTyped()
		{
			Debug.Log("Spell typed in FocusMode");
			OnFocusTimerComplete();
		}

		private void OnFocusTimerComplete()
		{
			FocusModeComplete?.Invoke();

			MMGameEvent.Trigger(GameDefine.FocusModeEvents.EventName, stringParameter: GameDefine.FocusModeEvents.State_TimeOut);
		}
	}
}
