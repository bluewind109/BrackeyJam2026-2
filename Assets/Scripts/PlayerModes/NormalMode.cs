using UnityEngine;

namespace PlayerModes
{
	public class NormalMode : PlayerMode
	{
		public NormalMode(Player player) : base(player)
		{
		}

		public override void Enter()
		{
			_player.RightMouseClicked += OnRightMouseClicked;
		}

		public override void Exit()
		{
			_player.RightMouseClicked -= OnRightMouseClicked;
		}

		public override void Update()
		{
			_player.GameUpdate();
		}

		private void OnRightMouseClicked()
		{
			
		}
	}
}
