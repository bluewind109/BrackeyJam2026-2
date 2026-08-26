using UnityEngine;

namespace EnemyStates
{
    public class ChaseState : EnemyState
    {
        private float _chaseDuration = 2f;
        private float _chaseTimer = 0f;

        private Enemy _enemy;
        private Player _player;
        private float _chaseSpeed = 1f;

        public void Initialize(Enemy enemy, Player player)
        {
            _enemy = enemy;
            _player = player;
        }

        public override void Enter()
        {
            Debug.Log("Entering Chase State");
            _chaseTimer = 0f;
        }

        public override void Exit()
        {
            // Logic for exiting the Chase state
        }

        public override void UpdateState()
        {
            ChasePlayer();

            _chaseTimer += Time.deltaTime;
            if (_chaseTimer >= _chaseDuration)
            {
                OnStateFinished();
            }
        }

        private void ChasePlayer()
        {
            if (_player == null) return;
            Vector3 direction = (_player.transform.position - _enemy.transform.position).normalized;
            _enemy.transform.position += direction * Time.deltaTime * _chaseSpeed;
        }
    }
}
