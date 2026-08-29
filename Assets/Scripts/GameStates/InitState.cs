using UnityEngine;

namespace GameStates
{
	public class InitState : GameState
	{
		private Player _player;
		private Enemy _enemy;

		public InitState(GameManager gameManager, Player player, Enemy enemy) : base(gameManager)
		{
			_player = player;
			_enemy = enemy;
		}

		public override void Enter()
		{
			SpellManager.Instance.Initialize();
			PlayerProjectileManager.Instance.Initialize();
			EnemyProjectileManager.Instance.Initialize();
			AoeAttackManager.Instance.Initialize();
			_enemy.Initialize(_player);
		}

		public override void Exit()
		{
		}

		public override void Update()
		{
			if (PlayerProjectileManager.Instance.IsInitialized &&
				EnemyProjectileManager.Instance.IsInitialized &&
				AoeAttackManager.Instance.IsInitialized)
			{
				_gameManager.SetState(_gameManager.IntroState);
			}
		}
	}
}
