using UnityEngine;

public class AoeAttack_EarthSpike : AoeAttack
{
    [SerializeField] private ParticleSystem _spikeVfxPrefab;

    ParticleSystem _spikeVfx;

    public override void Initialize(float delayDuration, int damage, float spawnRadius)
    {
        base.Initialize(delayDuration, damage, spawnRadius);
        _spikeVfx = Instantiate(_spikeVfxPrefab);
        _spikeVfx.transform.position = transform.position;
        var spikeShape = _spikeVfx.shape;
        spikeShape.radius = spawnRadius / 2; // Set the radius of the particle system to match the hitbox radius
    }

    protected override void ExecuteAoeAttack()
    {
        // Skip executing state
        StartPersisting();
    }

    protected override void StartPersisting()
    {
        base.StartPersisting();
        _spikeVfx?.Play();
    }
}
