using UnityEngine;

namespace EnemyStates
{
    public class SlamAttackState : EnemyState
    {
        private Player _player;
        private int _numberOfSlamAttacks = 8;

        public void Initialize(Player player, int numberOfSlamAttacks)
        {
            _player = player;
            _numberOfSlamAttacks = numberOfSlamAttacks;
        }

        public override void Enter()
        {
            Debug.Log("Entering Slam Attack State");
            SpawnAoeAttacksAroundPlayer();
            OnStateFinished();
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
        }

        // Spawn aoe attacks in random positions around the player in a circular pattern
        private void SpawnAoeAttacksAroundPlayer()
        {
            Vector3 playerPosition = _player.transform.position;
            float angleStep = 360f / _numberOfSlamAttacks;
            for (int i = 0; i < _numberOfSlamAttacks; i++)
            {
                float angle = i * angleStep;
                float radius = Random.Range(1f, 2f); // Distance from the player to spawn the AOE attacks
                Vector3 spawnPosition = playerPosition +
                    new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * radius;
                AoeAttackManager.Instance.SpawnAoeAttack(1f, 1, spawnPosition);
            }
        }
    }
}
