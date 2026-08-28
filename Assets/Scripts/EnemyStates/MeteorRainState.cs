using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates
{
    public class MeteorRainState : EnemyState
    {
        [SerializeField] private float _minSpawnRadius = 0.9f;
        [SerializeField] private float _maxSpawnRadius = 2.4f;
        [SerializeField] private float _baseDelayDuration = 1f;
        [SerializeField] private float _spawnDelayJitter = 0.35f;
        [SerializeField] private float _spawnStaggerMin = 0.03f;
        [SerializeField] private float _spawnStaggerMax = 0.12f;
        [SerializeField] private float _minSpawnDistanceBetweenAttacks = 0.65f;
        [SerializeField] private int _positionRetryCount = 6;
        [SerializeField] private int _meteorDamage = 1;

        private Player _player;
        private int _numberOfMeteors = 8;

        public void Initialize(Player player, int numberOfMeteors)
        {
            _player = player;
            _numberOfMeteors = numberOfMeteors;
        }

        public override void Enter()
        {
            Debug.Log("Entering Meteor Rain State");
            if (_player == null)
            {
                Debug.LogError("MeteorRainState requires a player reference before entering.");
                OnStateFinished();
                return;
            }

            SpawnMeteorsAroundPlayer();
            OnStateFinished();
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
        }

        private void SpawnMeteorsAroundPlayer()
        {
            if (_numberOfMeteors <= 0)
            {
                Debug.LogWarning("MeteorRainState skipped spawning because number of meteors is not positive.");
                return;
            }

            if (AoeAttackManager.Instance == null)
            {
                Debug.LogError("Cannot spawn meteor rain AOEs because AoeAttackManager.Instance is null.");
                return;
            }

            Vector3 playerPosition = _player.transform.position;
            List<Vector3> existingSpawnPositions = new List<Vector3>(_numberOfMeteors);
            float accumulatedStagger = 0f;

            float centerRandomDelay = Random.Range(0f, Mathf.Max(0f, _spawnDelayJitter));
            float centerDelayDuration = Mathf.Max(0f, _baseDelayDuration + centerRandomDelay);
            AoeAttackManager.Instance.SpawnAoeAttack(centerDelayDuration, _meteorDamage, playerPosition);
            existingSpawnPositions.Add(playerPosition);

            for (int i = 1; i < _numberOfMeteors; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(playerPosition, existingSpawnPositions);
                existingSpawnPositions.Add(spawnPosition);

                float randomDelay = Random.Range(0f, Mathf.Max(0f, _spawnDelayJitter));
                float delayDuration = Mathf.Max(0f, _baseDelayDuration + randomDelay + accumulatedStagger);
                AoeAttackManager.Instance.SpawnAoeAttack(delayDuration, _meteorDamage, spawnPosition);

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
