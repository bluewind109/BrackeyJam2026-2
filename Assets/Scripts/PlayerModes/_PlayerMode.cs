using UnityEngine;

namespace PlayerModes
{
	public abstract class PlayerMode
	{
		protected Player _player;
		public PlayerMode(Player player)
		{
			_player = player;
		}

		public abstract void Enter();
		public abstract void Exit();
		public abstract void Update();
	}
}
