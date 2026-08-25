using UnityEngine;

namespace EnemyStates
{
    public class CircleShotState : EnemyState
    {
        private int _numberOfShots = 8;
        private float _shotInterval = 0.5f;

        private float _idleDuration = 2f;
        private float _idleTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Circle Shot State");
            _idleTimer = 0f;
        }

        public override void Exit()
        {
            // Logic for exiting the Circle Shot state
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
