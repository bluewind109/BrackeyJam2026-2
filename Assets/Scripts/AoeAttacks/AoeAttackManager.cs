using UnityEngine;
using System.Collections.Generic;

public class AoeAttackManager : MonoBehaviour
{
    public static AoeAttackManager Instance { get; private set; }

    [SerializeField] private AoeAttack _aoeAttackMeteorPrefab;
    [SerializeField] private AoeAttack _aoeAttackEarthSpikePrefab;

    private List<AoeAttack> _meteorPool = new List<AoeAttack>();
    private List<AoeAttack> _earthSpikePool = new List<AoeAttack>();

    private const int METEOR_POOL_SIZE = 15;
    private const int EARTH_SPIKE_POOL_SIZE = 2;

    private bool _isInitialized = false;
    public bool IsInitialized => _isInitialized;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Initialize()
    {
        InitializePool();
        _isInitialized = true;
    }

    private void InitializePool()
    {
        for (int i = 0; i < METEOR_POOL_SIZE; i++)
        {
            CreateNewMeteorAttack();
        }
        for (int i = 0; i < EARTH_SPIKE_POOL_SIZE; i++)
        {
            CreateNewEarthSpikeAttack();
        }
    }

    private AoeAttack CreateNewMeteorAttack()
    {
        var newAttack = Instantiate(_aoeAttackMeteorPrefab, transform);
        newAttack.gameObject.SetActive(false);
        _meteorPool.Add(newAttack);
        return newAttack;
    }

    private AoeAttack CreateNewEarthSpikeAttack()
    {
        var newAttack = Instantiate(_aoeAttackEarthSpikePrefab, transform);
        newAttack.gameObject.SetActive(false);
        _earthSpikePool.Add(newAttack);
        return newAttack;
    }

    public AoeAttack SpawnMeteorAttack(
        float delayDuration, 
        int damage, 
        float spawnRadius,
        Vector3 spawnPosition,
        Vector3 directionToTarget)
    {
        var attack = GetFreeMeteorAttack();
        if (attack != null)
        {
            attack.transform.position = spawnPosition;
            if (attack is AoeAttack_Meteor meteorAttack)
            {
                meteorAttack.SetDirectionToTarget(directionToTarget);
            }
            attack.Initialize(delayDuration, damage, spawnRadius);
            return attack;
        }
        return null;
    }

    public AoeAttack GetFreeMeteorAttack()
    {
        foreach (var attack in _meteorPool)
        {
            if (!attack.gameObject.activeInHierarchy)
            {
                return attack;
            }
        }

        if (_aoeAttackMeteorPrefab == null)
        {
            Debug.LogError("AoeAttack Meteor prefab is not assigned in AoeAttackManager.");
            return null;
        }
        var newAttack = CreateNewMeteorAttack();
        return newAttack;
    }

    public AoeAttack SpawnEarthSpikeAttack(
        float delayDuration, 
        int damage, 
        float spawnRadius,
        Vector3 position)
    {
        var attack = GetFreeEarthSpikeAttack();
        if (attack != null)
        {
            attack.transform.position = position;
            attack.Initialize(delayDuration, damage, spawnRadius);
            return attack;
        }
        return null;
    }

    public AoeAttack GetFreeEarthSpikeAttack()
    {
        foreach (var attack in _earthSpikePool)
        {
            if (!attack.gameObject.activeInHierarchy)
            {
                return attack;
            }
        }

        if (_aoeAttackEarthSpikePrefab == null)
        {
            Debug.LogError("AoeAttack Earth Spike prefab is not assigned in AoeAttackManager.");
            return null;
        }
        var newAttack = CreateNewEarthSpikeAttack();
        return newAttack;
    }

    public void GameUpdate()
    {
        foreach (var attack in _meteorPool)
        {
            if (!attack.gameObject.activeSelf) continue;
            attack.GameUpdate();
        }
        foreach (var attack in _earthSpikePool)
        {
            if (!attack.gameObject.activeSelf) continue;
            attack.GameUpdate();
        }
    }
}
