using UnityEngine;

namespace PlayerModes
{
	public class FocusMode : PlayerMode
	{
		public FocusMode(Player player) : base(player)
		{
		}

		public override void Enter()
		{
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			_player.GameUpdate();
		}
	}
}
