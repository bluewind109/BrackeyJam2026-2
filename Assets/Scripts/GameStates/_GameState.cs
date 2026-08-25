using UnityEngine;

namespace GameStates
{
	public abstract class GameState
	{
		protected GameManager _gameManager;

		public GameState(GameManager gameManager)
		{
			_gameManager = gameManager;
		}

		public abstract void Enter();
		public abstract void Exit();
		public abstract void Update();
	}
}
