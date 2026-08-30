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
        [SerializeField] private List<MeteorRainStateConfig> _phaseConfigs = new List<MeteorRainStateConfig>();
        private MeteorRainStateConfig _currentStateConfig;

        private int _positionRetryCount = 6;
        private float _waitTimer = 0f;

        public override void Initialize(Enemy enemy, Player player)
        {
            base.Initialize(enemy, player);
            SetPhaseConfig(0);
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

           _waitTimer = 0f; 
           _enemy.EnemyDisplay.PlayCastAnimation(GameDefine.BossAttackEvents.State_Meteor);
            SpawnMeteorsAroundPlayer();
        }

        public override void Exit()
        {
        }

        public override void UpdateState()
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _currentStateConfig.BaseDelayDuration)
            {
                OnStateFinished();
            }
        }

        private void SpawnMeteorsAroundPlayer()
        {
            if (_currentStateConfig.NumberOfMeteors <= 0)
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
            Vector3 directionToPlayer = (playerPosition - _enemy.transform.position).normalized;
            List<Vector3> existingSpawnPositions = new List<Vector3>(_currentStateConfig.NumberOfMeteors);
            float accumulatedStagger = 0f;

            float centerRandomDelay = Random.Range(0f, Mathf.Max(0f, _currentStateConfig.SpawnDelayJitter));
            float centerDelayDuration = Mathf.Max(0f, _currentStateConfig.BaseDelayDuration + centerRandomDelay);
            AoeAttackManager.Instance.SpawnMeteorAttack(
                centerDelayDuration, 
                _currentStateConfig.MeteorDamage, 
                _currentStateConfig.SpawnRadius, 
                playerPosition, 
                directionToPlayer
            );
            existingSpawnPositions.Add(playerPosition);

            for (int i = 1; i < _currentStateConfig.NumberOfMeteors; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(playerPosition, existingSpawnPositions);
                existingSpawnPositions.Add(spawnPosition);

                float randomDelay = Random.Range(0f, Mathf.Max(0f, _currentStateConfig.SpawnDelayJitter));
                float delayDuration = Mathf.Max(0f, _currentStateConfig.BaseDelayDuration + randomDelay + accumulatedStagger);
                AoeAttackManager.Instance.SpawnMeteorAttack(
                    delayDuration, 
                    _currentStateConfig.MeteorDamage, 
                    _currentStateConfig.SpawnRadius, 
                    spawnPosition,
                    directionToPlayer
                );
                float staggerStep = Random.Range(
                    Mathf.Max(0f, _currentStateConfig.SpawnStaggerMin),
                    Mathf.Max(_currentStateConfig.SpawnStaggerMin, _currentStateConfig.SpawnStaggerMax));
                accumulatedStagger += staggerStep;
            }
        }

        private Vector3 GetSpawnPosition(Vector3 playerPosition, List<Vector3> existingSpawnPositions)
        {
            int retries = Mathf.Max(1, _positionRetryCount);
            float minDistance = Mathf.Max(0f, _currentStateConfig.MinSpawnDistanceBetweenAttacks);
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
            float minRadius = Mathf.Max(0f, Mathf.Min(_currentStateConfig.MinSpawnRadius, _currentStateConfig.MaxSpawnRadius));
            float maxRadius = Mathf.Max(minRadius, _currentStateConfig.MaxSpawnRadius);
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

        public void SetPhaseConfig(int phaseIndex)
        {
            if (phaseIndex < 0 || _phaseConfigs.Count == 0)
            {
                Debug.LogWarning($"Invalid phase index {phaseIndex}. Using default config.");
                return;
            }

            if (phaseIndex >= _phaseConfigs.Count)
            {
                Debug.LogWarning($"Phase index {phaseIndex} exceeds available configs. Using last config.");
                phaseIndex = _phaseConfigs.Count - 1;
            }

            _currentStateConfig = _phaseConfigs[phaseIndex];
        }
    }
}
