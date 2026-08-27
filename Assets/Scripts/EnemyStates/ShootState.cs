using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class ShootState : EnemyState
    {
        [SerializeField] private List<ShootPatternInfo> _possiblePatterns;

        private int _currentShotCount = 0;
        private float _shotTimer = 0f;
        private ShootPatternInfo _currentPatternInfo;
        private Pattern _currentPattern;

        private Player _player;

        public void Initialize(Player player)
        {
            _player = player;
        }

        public override void Enter()
        {
            Debug.Log("Entering Shoot State");
            _shotTimer = 0f;
            _currentShotCount = 0;
            _currentPatternInfo = GetRandomPatternInfo();
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
                if (_currentShotCount >= _currentPatternInfo.Config.NumberOfShots)
                {
                    OnStateFinished();
                }
            }
        }

        private ShootPatternInfo GetRandomPatternInfo()
        {
            if (_possiblePatterns.Count == 0)
            {
                Debug.LogError("No shoot patterns available.");
                return null;
            }

            int randomIndex = Random.Range(0, _possiblePatterns.Count);
            return _possiblePatterns[randomIndex];
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
    }

}
