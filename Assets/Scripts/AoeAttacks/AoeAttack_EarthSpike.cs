using UnityEngine;

public class AoeAttack_EarthSpike : AoeAttack
{
    [SerializeField] private SpriteRenderer _spikeSpriteRenderer;
    [SerializeField] private float _baseScale = 1.0f;
    [SerializeField] private float _scaleOffset = 0.5f;

    

    public override void Initialize(float delayDuration, int damage, float spawnRadius)
    {
        base.Initialize(delayDuration, damage, spawnRadius);
        _spikeSpriteRenderer.gameObject.SetActive(false);
        _spikeSpriteRenderer.transform.localScale = 
            Vector3.one * (_baseScale + Random.Range(-_scaleOffset, _scaleOffset));
    }

    protected override void ExecuteAoeAttack()
    {
        base.ExecuteAoeAttack();
        _spikeSpriteRenderer.gameObject.SetActive(true);
    }

    private void SpawnSpikeVisuals()
    {
        _spikeSpriteRenderer.gameObject.SetActive(true);
    }
}
