using UnityEngine;

namespace EnemyStates
{
	public class RecoverState : EnemyState
	{
        private float _recoveryDuration = 1f;
        private float _recoveryTimer = 0f;

		public override void Enter()
		{
            Debug.Log("Entering Recover State");
			_recoveryTimer = 0f;
		}

		public override void Exit()
		{
		}

		public override void UpdateState()
		{
			_recoveryTimer += Time.deltaTime;
			if (_recoveryTimer >= _recoveryDuration)
			{
				OnStateFinished();
			}
		}
	}
}
