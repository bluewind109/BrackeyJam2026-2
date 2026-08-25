using UnityEngine;

namespace EnemyStates
{
    public class IdleState : EnemyState
    {
        private float _idleDuration = 2f;
        private float _idleTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Idle State");
            _idleTimer = 0f;
        }

        public override void Exit()
        {
            // Logic for exiting the Idle state
        }

        public override void UpdateState()
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleDuration)
            {
                OnStateFinished();
            }
        }
    }
}
