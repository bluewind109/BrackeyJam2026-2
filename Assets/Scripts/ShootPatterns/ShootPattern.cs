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
}
