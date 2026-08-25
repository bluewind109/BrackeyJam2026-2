using UnityEngine;

namespace EnemyStates
{
    public class DashState : EnemyState
    {
        private float _dashDuration = 1f;
        private float _dashTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Dash State");
            _dashTimer = 0f;
        }

        public override void Exit()
        {
            // Logic for exiting the Dash state
        }

        public override void UpdateState()
        {
            _dashTimer += Time.deltaTime;
            if (_dashTimer >= _dashDuration)
            {
                OnStateFinished();
            }
        }
    }
}
