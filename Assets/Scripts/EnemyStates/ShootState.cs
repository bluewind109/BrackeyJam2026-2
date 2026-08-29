using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class ShootState : EnemyState
    {
        [SerializeField] private List<ShootStateConfig> _phaseConfigs;
        private ShootStateConfig _currentStateConfig;

        private int _currentShotCount = 0;
        private float _shotTimer = 0f;
        private ShootPatternInfo _currentPatternInfo;
        private Pattern _currentPattern;

        private Player _player;

        public void Initialize(Player player)
        {
            _player = player;
           SetPhaseConfig(0);
        }

        public override void Enter()
        {
            Debug.Log("Entering Shoot State");
            _shotTimer = 0f;
            _currentShotCount = 0;
            _currentPatternInfo = _currentStateConfig.GetRandomPatternInfo();
            Debug.Log($"Selected Pattern: <color=yellow>{_currentPatternInfo.PatternType}</color>");
            _currentPattern = PatternFactory.CreatePattern(_currentPatternInfo);
        }

        public override void Exit()
        {
            Debug.Log("Exiting Shoot State");
        }

        public override void UpdateState()
        {
            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _currentPatternInfo.Config.ShotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                // Shoot(_currentShotCount * (360f / _numberOfShots));
                Shoot();
                if (_currentPatternInfo.Config is IShotBasedPattern shotBasedPattern &&
                    _currentShotCount >= shotBasedPattern.NumberOfShots)
                {
                    OnStateFinished();
                }
            }
        }

        private void Shoot()
        {
            if (_currentPattern == null) return;

            if (_currentPattern is IStaticPattern staticPattern)
            {
                staticPattern.Shoot(transform.position);
            }
            else if (_currentPattern is IDirectionalPattern directionalPattern)
            {
                Vector3 direction = (_player.transform.position - transform.position).normalized;
                directionalPattern.Shoot(transform.position, direction);
            }
        }

        public void SetPhaseConfig(int phaseIndex)
        {
            if (phaseIndex < 0 || _phaseConfigs == null || _phaseConfigs.Count == 0)
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
