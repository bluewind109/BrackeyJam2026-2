using UnityEngine;

namespace EnemyStates
{
	public class PhaseTransitionState : EnemyState
	{
        private float _transitionDuration = 2f;
        private float _transitionTimer = 0f;

		public override void Enter()
        {
            Debug.Log("Entering Phase Transition State");
            _transitionTimer = 0f;

            // TODO play scream anim
        }

		public override void Exit()
		{
		}

		public override void UpdateState()
        {
            _transitionTimer += Time.deltaTime;
            if (_transitionTimer >= _transitionDuration)
            {
                OnStateFinished();
            }
        }
	}
}
