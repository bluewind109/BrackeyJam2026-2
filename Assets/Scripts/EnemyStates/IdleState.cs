using UnityEngine;
using MoreMountains.Tools;
namespace EnemyStates
{
    public class IdleState : EnemyState
    {
        private float _idleDuration = 3f;
        private float _idleTimer = 0f;

        public override void Enter()
        {
            Debug.Log("Entering Idle State");
            _idleTimer = 0f;
        }

        public override void Exit()
        {
            // Logic for exiting the Idle state
        }

        public override void UpdateState()
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleDuration)
            {
                OnStateFinished();
                
                MMGameEvent.Trigger(GameDefine.BossEvents.EventName, stringParameter: GameDefine.BossEvents.State_SpawnFinished);
            }
        }
    }
}
