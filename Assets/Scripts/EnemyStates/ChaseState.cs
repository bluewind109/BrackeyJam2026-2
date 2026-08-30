using UnityEngine;

namespace EnemyStates
{
    public class ChaseState : EnemyState
    {
        [SerializeField] private ChaseStateConfig _config;

        private float _chaseTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Chase State");
            _chaseTimer = 0f;
            _enemy.EnemyDisplay.PlayIdleAnimation();
        }

        public override void Exit()
        {
            // Logic for exiting the Chase state
        }

        public override void UpdateState()
        {
            Vector3 chaseDirection = ChasePlayer();
            _enemy.EnemyDisplay.UpdateMoving(chaseDirection);

            _chaseTimer += Time.deltaTime;
            if (_chaseTimer >= _config.ChaseDuration)
            {
                OnStateFinished();
            }
        }

        private Vector3 ChasePlayer()
        {
            if (_player == null) return Vector3.zero;
            Vector3 direction = (_player.transform.position - _enemy.transform.position).normalized;
            _enemy.Move(direction * Time.deltaTime * _config.ChaseSpeed);
            return direction;
        }
    }
}
