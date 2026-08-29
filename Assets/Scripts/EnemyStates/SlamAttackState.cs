using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates
{
    public class SlamAttackState : EnemyState
    {
        [SerializeField] private List<SlamAttackStateConfig> _phaseConfigs = new List<SlamAttackStateConfig>();
        private SlamAttackStateConfig _currentStateConfig;


        public void Initialize()
        {
            SetPhaseConfig(0);
        }

        public override void Enter()
        {
            Debug.Log("Entering Slam Attack State");
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
                _currentStateConfig.SlamAttackDelay,
                _currentStateConfig.SlamAttackDamage,
                _currentStateConfig.SlamAttackRadius,
                slamAttackPosition
            );
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
