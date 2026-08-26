using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileManager : MonoBehaviour
{
	[SerializeField] private EnemyProjectile _projectilePrefab;

	private Stack<EnemyProjectile> _projectilePool = new Stack<EnemyProjectile>();

	private const int POOL_SIZE = 50;

	private bool _isInitialized = false;
	public bool IsInitialized => _isInitialized;

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
			_projectilePool.Push(projectile);
		}
	}

	private EnemyProjectile CreateNewProjectile()
	{
		var newProjectile = Instantiate(_projectilePrefab, transform);
		newProjectile.OnReleased += OnProjectileReleased;
		return newProjectile;
	}

	public EnemyProjectile GetFreeProjectile()
	{
		if (_projectilePool.Count > 0)
		{
			return _projectilePool.Pop();
		}

		if (_projectilePrefab == null)
		{
			Debug.LogError("Projectile prefab is not assigned in EnemyProjectileManager.");
			return null;
		}
		var newProjectile = CreateNewProjectile();
		return newProjectile;
	}

	private void OnProjectileReleased(EnemyProjectile projectile)
	{
		projectile.OnReleased -= OnProjectileReleased;
		projectile.gameObject.SetActive(false);
		_projectilePool.Push(projectile);
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
