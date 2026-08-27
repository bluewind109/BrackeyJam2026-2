using ShootPatterns;
using UnityEngine;

namespace EnemyStates
{
    public class RadialShootState : EnemyState
    {
        [SerializeField] private RadialPatternConfig _shootPatternConfig;

		private int _currentShotCount = 0;
        private float _shotTimer = 0f;
        private RadialPattern _radialPattern;

        public override void Enter()
        {
            Debug.Log("Entering Radial Shoot State");
            _shotTimer = 0f;
			_currentShotCount = 0;
			_radialPattern = new RadialPattern();
        }

        public override void Exit()
        {
            // Logic for exiting the Radial Shoot state
        }

        public override void UpdateState()
        {
            _shotTimer += Time.deltaTime;
            if (_shotTimer >= _shootPatternConfig.ShotInterval)
            {
                _shotTimer = 0f;
                _currentShotCount++;
                // Shoot(_currentShotCount * (360f / _numberOfShots));
                Shoot();
                if (_currentShotCount >= _shootPatternConfig.NumberOfShots)
                {
                    OnStateFinished();
                }
            }
        }

		private void Shoot()
		{
            _radialPattern.Shoot(
                _shootPatternConfig.ProjectilesPerShot,
                transform.position, 
                _shootPatternConfig.ProjectileSpeed,
                _shootPatternConfig.Damage
            );
		}
    }
}
