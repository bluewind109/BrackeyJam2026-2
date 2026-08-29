using System;
using UnityEngine;

namespace EnemyStates
{
    public abstract class EnemyState : MonoBehaviour
    {
        public event Action StateFinished;

        protected Enemy _enemy;
        protected Player _player;

        public virtual void Initialize(Enemy enemy, Player player)
        {
            _enemy = enemy;
            _player = player;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void UpdateState();

        protected void OnStateFinished()
        {
            StateFinished?.Invoke();
        }
    }
}