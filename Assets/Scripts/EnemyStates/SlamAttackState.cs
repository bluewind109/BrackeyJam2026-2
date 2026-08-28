using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates
{
    public class SlamAttackState : EnemyState
    {
        [SerializeField] private float _minSpawnRadius = 0.9f;
        [SerializeField] private float _maxSpawnRadius = 2.4f;
        [SerializeField] private float _baseDelayDuration = 1f;
        [SerializeField] private float _spawnDelayJitter = 0.35f;
        [SerializeField] private float _spawnStaggerMin = 0.03f;
        [SerializeField] private float _spawnStaggerMax = 0.12f;
        [SerializeField] private float _minSpawnDistanceBetweenAttacks = 0.65f;
        [SerializeField] private int _positionRetryCount = 6;
        [SerializeField] private int _slamDamage = 1;

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
            if (_player == null)
            {
                Debug.LogError("SlamAttackState requires a player reference before entering.");
                OnStateFinished();
                return;
            }

            SpawnAoeAttacksAroundPlayer();
            OnStateFinished();
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
        }

        private void SpawnAoeAttacksAroundPlayer()
        {
            if (_numberOfSlamAttacks <= 0)
            {
                Debug.LogWarning("SlamAttackState skipped spawning because number of slam attacks is not positive.");
                return;
            }

            if (AoeAttackManager.Instance == null)
            {
                Debug.LogError("Cannot spawn slam AOEs because AoeAttackManager.Instance is null.");
                return;
            }

            Vector3 playerPosition = _player.transform.position;
            List<Vector3> existingSpawnPositions = new List<Vector3>(_numberOfSlamAttacks);
            float accumulatedStagger = 0f;

            for (int i = 0; i < _numberOfSlamAttacks; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(playerPosition, existingSpawnPositions);
                existingSpawnPositions.Add(spawnPosition);

                float randomDelay = Random.Range(0f, Mathf.Max(0f, _spawnDelayJitter));
                float delayDuration = Mathf.Max(0f, _baseDelayDuration + randomDelay + accumulatedStagger);
                AoeAttackManager.Instance.SpawnAoeAttack(delayDuration, _slamDamage, spawnPosition);

                float staggerStep = Random.Range(
                    Mathf.Max(0f, _spawnStaggerMin),
                    Mathf.Max(_spawnStaggerMin, _spawnStaggerMax));
                accumulatedStagger += staggerStep;
            }
        }

        private Vector3 GetSpawnPosition(Vector3 playerPosition, List<Vector3> existingSpawnPositions)
        {
            int retries = Mathf.Max(1, _positionRetryCount);
            float minDistance = Mathf.Max(0f, _minSpawnDistanceBetweenAttacks);
            Vector3 fallbackPosition = SampleSpawnPosition(playerPosition);

            for (int attempt = 0; attempt < retries; attempt++)
            {
                Vector3 candidate = SampleSpawnPosition(playerPosition);
                fallbackPosition = candidate;
                if (IsFarEnoughFromExistingSpawns(candidate, existingSpawnPositions, minDistance))
                {
                    return candidate;
                }

                minDistance *= 0.85f;
            }

            return fallbackPosition;
        }

        private Vector3 SampleSpawnPosition(Vector3 playerPosition)
        {
            float minRadius = Mathf.Max(0f, Mathf.Min(_minSpawnRadius, _maxSpawnRadius));
            float maxRadius = Mathf.Max(minRadius, _maxSpawnRadius);
            float angleRad = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(minRadius, maxRadius);

            Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * radius;
            return playerPosition + offset;
        }

        private bool IsFarEnoughFromExistingSpawns(
            Vector3 candidate,
            List<Vector3> existingSpawnPositions,
            float minDistance)
        {
            float minDistanceSqr = minDistance * minDistance;
            for (int i = 0; i < existingSpawnPositions.Count; i++)
            {
                if ((candidate - existingSpawnPositions[i]).sqrMagnitude < minDistanceSqr)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
