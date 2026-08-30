using UnityEngine;

namespace EnemyStates
{
	public class DeathState : EnemyState
	{
		public override void Enter()
        {
            Debug.Log("Entering Death State");
            _enemy.EnemyDisplay.PlayIdleAnimation();
        }

		public override void Exit()
		{
		}

		public override void UpdateState()
		{
		}
	}
}
