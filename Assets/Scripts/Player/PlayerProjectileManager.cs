using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileManager : MonoBehaviour
{
    public static PlayerProjectileManager Instance { get; private set; }

    [SerializeField] private FireBall _fireBallPrefab;
    [SerializeField] private IceLance _iceLancePrefab;

    private List<FireBall> _fireBallPool = new List<FireBall>();
    private List<IceLance> _iceLancePool = new List<IceLance>();

    private const int POOL_SIZE = 5;
    private bool _isInitialized = false;
    public bool IsInitialized => _isInitialized;

    void Awake()
    {
        Instance = this;
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
            var fireBall = CreateNewFireBall();
            _fireBallPool.Add(fireBall);

            var iceLance = CreateNewIceLance();
            _iceLancePool.Add(iceLance);
        }
    }

    private FireBall CreateNewFireBall()
    {
        var newFireBall = Instantiate(_fireBallPrefab, transform);
        newFireBall.gameObject.SetActive(false);
        return newFireBall;
    }

    private IceLance CreateNewIceLance()
    {
        var newIceLance = Instantiate(_iceLancePrefab, transform);
        newIceLance.gameObject.SetActive(false);
        return newIceLance;
    }

    public Projectile SpawnProjectile<T>(Vector3 position, Vector3 direction, float speed, int damage) where T : Projectile
    {
        List<T> pool = typeof(T) == typeof(FireBall) ? _fireBallPool as List<T> : _iceLancePool as List<T>;
        var projectile = GetFreeProjectile(pool);
        if (projectile != null)
        {
            projectile.transform.position = position;
            projectile.Initialize(direction, speed, damage, "Enemy");
            return projectile;
        }
        return null;
    }

    private Projectile GetFreeProjectile<T>(List<T> pool) where T : Projectile
    {
        foreach (var projectile in pool)
        {
            if (!projectile.gameObject.activeInHierarchy)
            {
                return projectile;
            }
        }

        // If no free projectile is found, create a new one and add it to the pool
        if (typeof(T) == typeof(FireBall))
        {
            var newFireBall = CreateNewFireBall();
            _fireBallPool.Add(newFireBall);
            return newFireBall;
        }
        else if (typeof(T) == typeof(IceLance))
        {
            var newIceLance = CreateNewIceLance();
            _iceLancePool.Add(newIceLance);
            return newIceLance;
        }

        return null;
    }

    public void GameUpdate()
    {
        foreach (var fireBall in _fireBallPool)
        {
            if (!fireBall.IsActive) continue;
            fireBall.GameUpdate();
        }

        foreach (var iceLance in _iceLancePool)
        {
            if (!iceLance.IsActive) continue;
            iceLance.GameUpdate();
        }
    }

}
