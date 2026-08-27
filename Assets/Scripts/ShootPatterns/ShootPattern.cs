using System;
using UnityEngine;

namespace ShootPatterns
{
	public enum ShootPatternType
	{
		Radial,
		Spiral,
		Fan,
		Sideway
	}

	public abstract class Pattern
	{
		public event Action OnShootComplete;
	}

	public interface IStaticPattern
	{
		void Shoot(Vector3 position);
	}

	public interface IDirectionalPattern
	{
		void Shoot(Vector3 position, Vector3 direction);
	}
}
