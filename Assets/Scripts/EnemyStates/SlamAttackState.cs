using UnityEngine;

namespace EnemyStates
{
	public class SlamAttackState : EnemyState
	{
        private float _slamAttackDelay = 1f;
        private float _slamAttackTimer = 0f;

        private float _slamAttackPersistenceDuration = 0.1f;
        private float _slamAttackPersistenceTimer = 0f;

        private int _slamAttackDamage = 1;
        private float _slamAttackRadius = 16f;


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
                _slamAttackDelay, 
                _slamAttackDamage, 
                _slamAttackRadius,
                slamAttackPosition
            );
        }
	}
}
