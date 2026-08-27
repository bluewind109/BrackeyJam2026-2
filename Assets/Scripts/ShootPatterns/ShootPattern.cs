using System;
using UnityEngine;

namespace ShootPatterns
{
	public abstract class Pattern
	{
		public event Action OnShootComplete;
	}
}
