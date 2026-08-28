using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates
{
    /// <summary>
    /// In SpawnMeteorsAroundPlayer, stagger is controlled by 3 pieces:<br/>
    /// <b>_spawnStaggerMin</b> and <b>_spawnStaggerMax</b> <br/>
    /// These define the random extra time added between each meteor. <br/>
    ///  <br/>
    /// <b>accumulatedStagger</b> <br/>
    /// This starts at 0, then grows every loop by a random step: <br/>
    /// staggerStep = Random.Range(_spawnStaggerMin, _spawnStaggerMax) <br/>
    /// accumulatedStagger += staggerStep <br/>
    ///  <br/>
    /// Then each meteor delay is: <br/>
    /// delayDuration = _baseDelayDuration + randomDelay + accumulatedStagger <br/>
    /// So later meteors always tend to land later than earlier ones,  <br/>
    /// creating a “rain over time” effect instead of all AOEs appearing at once. <br/>
    ///  <br/>
    /// Also: <br/>
    /// _spawnDelayJitter adds per-meteor randomness. <br/>
    /// The center meteor currently uses only base + jitter (no accumulated stagger). <br/>
    /// </summary>
    public class MeteorRainState : EnemyState
    {
        [SerializeField] private MeteorRainStateConfig _config;
        [SerializeField] private int _positionRetryCount = 6;

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

            float centerRandomDelay = Random.Range(0f, Mathf.Max(0f, _config.SpawnDelayJitter));
            float centerDelayDuration = Mathf.Max(0f, _config.BaseDelayDuration + centerRandomDelay);
            AoeAttackManager.Instance.SpawnAoeAttack(centerDelayDuration, _config.MeteorDamage, _config.SpawnRadius, playerPosition);
            existingSpawnPositions.Add(playerPosition);

            for (int i = 1; i < _numberOfMeteors; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(playerPosition, existingSpawnPositions);
                existingSpawnPositions.Add(spawnPosition);

                float randomDelay = Random.Range(0f, Mathf.Max(0f, _config.SpawnDelayJitter));
                float delayDuration = Mathf.Max(0f, _config.BaseDelayDuration + randomDelay + accumulatedStagger);
                AoeAttackManager.Instance.SpawnAoeAttack(delayDuration, _config.MeteorDamage, _config.SpawnRadius, spawnPosition);

                float staggerStep = Random.Range(
                    Mathf.Max(0f, _config.SpawnStaggerMin),
                    Mathf.Max(_config.SpawnStaggerMin, _config.SpawnStaggerMax));
                accumulatedStagger += staggerStep;
            }
        }

        private Vector3 GetSpawnPosition(Vector3 playerPosition, List<Vector3> existingSpawnPositions)
        {
            int retries = Mathf.Max(1, _positionRetryCount);
            float minDistance = Mathf.Max(0f, _config.MinSpawnDistanceBetweenAttacks);
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
            float minRadius = Mathf.Max(0f, Mathf.Min(_config.MinSpawnRadius, _config.MaxSpawnRadius));
            float maxRadius = Mathf.Max(minRadius, _config.MaxSpawnRadius);
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
