using System;
using UnityEngine;

namespace EnemyStates
{
    public abstract class EnemyState : MonoBehaviour
    {
        public event Action StateFinished;

        public abstract void Enter();
        public abstract void Exit();
        public abstract void UpdateState();

        protected void OnStateFinished()
        {
            StateFinished?.Invoke();
        }
    }
}