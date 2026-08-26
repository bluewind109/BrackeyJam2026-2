using UnityEngine;

namespace GameStates
{
	public class InitState : GameState
	{

		public InitState(GameManager gameManager) : base(gameManager)
		{
		}

		public override void Enter()
		{
			EnemyProjectileManager.Instance.Initialize();
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			if (EnemyProjectileManager.Instance.IsInitialized)
			{
				_gameManager.SetState(_gameManager.IntroState);
			}
		}
	}
}
