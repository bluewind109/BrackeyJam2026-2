using UnityEngine;

namespace GameStates
{
	public class IntroState : GameState
	{
		public IntroState(GameManager gameManager) : base(gameManager)
		{
		}

		public override void Enter()
		{
			// TODO show countdown UI
			_gameManager.SetState(_gameManager.GameplayState);
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
		}
	}
}
