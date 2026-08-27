using UnityEngine;

namespace EnemyStates
{
	public class SpawnState : EnemyState
	{
        private SpriteRenderer _bossSpriteRenderer;

        private float _fadeInDuration = 2f; // Duration for the fade-in effect
        private float _fadeInTimer = 0f;

        public void Initialize(SpriteRenderer bossSpriteRenderer)
        {
            _bossSpriteRenderer = bossSpriteRenderer;
        }

		public override void Enter()
        {
            Debug.Log("Entering Spawn State");
            _bossSpriteRenderer.color = new Color(
                _bossSpriteRenderer.color.r, 
                _bossSpriteRenderer.color.g, 
                _bossSpriteRenderer.color.b, 
                0f
            );
        }

		public override void Exit()
		{
		}

		public override void UpdateState()
        {
            _bossSpriteRenderer.color = new Color(
                _bossSpriteRenderer.color.r, 
                _bossSpriteRenderer.color.g, 
                _bossSpriteRenderer.color.b, 
                Mathf.Min(_bossSpriteRenderer.color.a + Time.deltaTime / _fadeInDuration, 1f)
            ); // Fade in the boss sprite

            if (_bossSpriteRenderer.color.a >= 1f)
            {
                OnStateFinished();
            }
        }
	}
}
