using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileManager : MonoBehaviour
{
	public static EnemyProjectileManager Instance { get; private set; }

	[SerializeField] private EnemyProjectile _projectilePrefab;

	private List<Projectile> _projectilePool = new List<Projectile>();

	private const int POOL_SIZE = 100;

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
			var projectile = CreateNewProjectile();
			_projectilePool.Add(projectile);
		}
	}

	private EnemyProjectile CreateNewProjectile()
	{
		var newProjectile = Instantiate(_projectilePrefab, transform);
		newProjectile.OnReleased += OnProjectileReleased;
		newProjectile.gameObject.SetActive(false);
		_projectilePool.Add(newProjectile);
		return newProjectile;
	}

	public Projectile SpawnProjectile(Vector3 position, Vector3 direction, float speed)
	{
		var projectile = GetFreeProjectile();
		if (projectile != null)
		{
			projectile.transform.position = position;
			projectile.Initialize(direction, speed, "Player");
			return projectile;
		}
		return null;
	}

	private Projectile GetFreeProjectile()
	{
		foreach (var projectile in _projectilePool)
		{
			if (!projectile.IsActive)
			{
				return projectile;
			}
		}

		if (_projectilePrefab == null)
		{
			Debug.LogError("Projectile prefab is not assigned in EnemyProjectileManager.");
			return null;
		}
		var newProjectile = CreateNewProjectile();
		return newProjectile;
	}

	private void OnProjectileReleased(Projectile projectile)
	{
		projectile.OnReleased -= OnProjectileReleased;
	}

	public void GameUpdate()
	{
		foreach (var projectile in _projectilePool)
		{
			if (!projectile.IsActive) continue;
			projectile.GameUpdate();
		}
	}
}
