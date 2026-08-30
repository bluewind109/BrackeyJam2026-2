using UnityEngine;

namespace EnemyStates
{
	public class RecoverState : EnemyState
	{
		[SerializeField] private RecoverStateConfig _config;

        private float _recoveryTimer = 0f;

		public override void Enter()
		{
            Debug.Log("Entering Recover State");
			_recoveryTimer = 0f;
            _enemy.EnemyDisplay.PlayIdleAnimation();
		}

		public override void Exit()
		{
		}

		public override void UpdateState()
		{
			_recoveryTimer += Time.deltaTime;
			if (_recoveryTimer >= _config.RecoverDuration)
			{
				OnStateFinished();
			}
		}
	}
}
