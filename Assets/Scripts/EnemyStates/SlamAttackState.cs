using UnityEngine;

namespace EnemyStates
{
	public class SlamAttackState : EnemyState
	{
        [SerializeField] private SlamAttackStateConfig _config;

        private float _slamAttackTimer = 0f;
        private float _slamAttackPersistenceTimer = 0f;

		public override void Enter()
		{
            Debug.Log("Entering Slam Attack State");
			_slamAttackTimer = 0f;
            _slamAttackPersistenceTimer = 0f;

            if (AoeAttackManager.Instance == null)
            {
                Debug.LogError("Cannot perform slam attack because AoeAttackManager.Instance is null.");
                OnStateFinished();
                return;
            }

            ExecuteSlamAttack();
            OnStateFinished();
        }

		public override void Exit()
		{
		}

		public override void UpdateState()
		{
		}

        private void ExecuteSlamAttack()
        {
            if (AoeAttackManager.Instance == null)
            {
                return;
            }

            Vector3 slamAttackPosition = transform.position;
            AoeAttack slamAttack = AoeAttackManager.Instance.SpawnAoeAttack(
                _config.SlamAttackDelay, 
                _config.SlamAttackDamage, 
                _config.SlamAttackRadius,
                slamAttackPosition
            );
        }
	}
}
