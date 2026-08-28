using UnityEngine;
using System.Collections.Generic;

public class AoeAttackManager : MonoBehaviour
{
    public static AoeAttackManager Instance { get; private set; }

    [SerializeField] private AoeAttack _aoeAttackPrefab;

    private List<AoeAttack> _attackPool = new List<AoeAttack>();

    private const int POOL_SIZE = 20;

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
        for (int i = 0; i < POOL_SIZE; i++)
        {
            CreateNewAttack();
        }
    }

    private AoeAttack CreateNewAttack()
    {
        var newAttack = Instantiate(_aoeAttackPrefab, transform);
        newAttack.gameObject.SetActive(false);
        _attackPool.Add(newAttack);
        return newAttack;
    }

    public AoeAttack SpawnAoeAttack(float delayDuration, int damage, Vector3 position)
    {
        var attack = GetFreeAttack();
        if (attack != null)
        {
            attack.transform.position = position;
            attack.Initialize(delayDuration, damage);
            return attack;
        }
        return null;
    }

    public AoeAttack GetFreeAttack()
    {
        foreach (var attack in _attackPool)
        {
            if (!attack.gameObject.activeInHierarchy)
            {
                return attack;
            }
        }

        if (_aoeAttackPrefab == null)
        {
            Debug.LogError("AoeAttack prefab is not assigned in AoeAttackManager.");
            return null;
        }
        var newAttack = CreateNewAttack();
        return newAttack;
    }

    public void GameUpdate()
    {
        foreach (var attack in _attackPool)
        {
            if (!attack.gameObject.activeSelf) continue;
            attack.GameUpdate();
        }
    }
}
