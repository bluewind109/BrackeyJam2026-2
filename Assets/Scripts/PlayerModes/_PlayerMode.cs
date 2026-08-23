using UnityEngine;

namespace PlayerModes
{
	public abstract class PlayerMode
	{
		public abstract void Enter();
		public abstract void Exit();
		public abstract void Update();
	}
}
