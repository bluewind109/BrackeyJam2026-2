using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class AoeAttack_Meteor : AoeAttack
{
    [SerializeField] private SpriteRenderer _meteorSpriteRenderer;
    [SerializeField] private ParticleSystem _impactVfxPrefab;

    [SerializeField] MMF_Player _feedback_casting;
    [SerializeField] MMF_Player _feedback_impact;

    [SerializeField] private float _fallDuration = 0.5f;
    [SerializeField] private float _baseScale = 1.0f;
    [SerializeField] private float _scaleOffset = 0.5f;

    ParticleSystem _impactVfx;
    private bool _hasImpacted = false;

    private float _fallTimer = 0.0f;
    private Vector3 _startPosition = new Vector3(3f, 6f, 0);
    private Vector3 _targetPosition = new Vector3(0f, 0f, 0f);

    private Vector3 _directionToTarget;

    public override void Initialize(float delayDuration, int damage, float spawnRadius)
    {
        base.Initialize(delayDuration, damage, spawnRadius);
        _fallTimer = 0.0f;
        _impactVfx = Instantiate(_impactVfxPrefab);
        _impactVfx.transform.position = transform.position;
        var impactShape = _impactVfx.shape;
        impactShape.radius = spawnRadius / 2; // Set the radius of the particle system to match the hitbox radius

        _meteorSpriteRenderer.gameObject.SetActive(false);
        _meteorSpriteRenderer.transform.localScale = Vector3.one * (_baseScale + Random.Range(-_scaleOffset, _scaleOffset));

        _hasImpacted = false;
    }

    public void SetDirectionToTarget(Vector3 direction)
    {
        _directionToTarget = direction.normalized;
        if (_directionToTarget.x > 0)
        {
            _startPosition = new Vector3(-3f, 6f, 0);
        }
        else
        {
            _startPosition = new Vector3(3f, 6f, 0);
        }
    }

    protected override void ExecuteAoeAttack()
    {
        base.ExecuteAoeAttack();
        _meteorSpriteRenderer.gameObject.SetActive(true);
        _feedback_casting?.PlayFeedbacks();
    }

	protected override void UpdateExecuting(float deltaTime)
	{
		base.UpdateExecuting(deltaTime);
        UpdateMeteorVisuals(deltaTime);
	}

    // Meteor falls from start position to target position over time
    private void UpdateMeteorVisuals(float deltaTime)
    {
        _fallTimer += deltaTime;
        float t = Mathf.Clamp01(_fallTimer / _fallDuration);
        _meteorSpriteRenderer.transform.localPosition = Vector3.Lerp(_startPosition, _targetPosition, t);
        _meteorSpriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, t * 360f); // Rotate meteor as it falls

        if (t >= 1.0f)
        {
            // Meteor has reached the target position, transition to persisting state
           StartPersisting();
           // TODO: Add impact effects, sound when the meteor hits the ground.
            _impactVfx?.Play();
            if(_hasImpacted == false)
            {
                _hasImpacted = true;
                _feedback_impact?.PlayFeedbacks();
            }
        }
    }
}
