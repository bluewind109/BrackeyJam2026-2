using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStates
{
	public class GameOverState : GameState
	{
		public GameOverState(GameManager gameManager) : base(gameManager)
		{
		}

		public override void Enter()
		{
			_ = TransitionToOutroScene();
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
		}

		private async UniTask TransitionToOutroScene()
		{
			await UniTask.Delay(2000); // Wait for 2 seconds before transitioning
			SceneManager.LoadScene("IntroScene");
		}
	}
}
