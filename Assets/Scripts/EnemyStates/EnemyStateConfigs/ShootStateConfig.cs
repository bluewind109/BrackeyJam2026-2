using System.Collections.Generic;
using ShootPatterns;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootStateConfig", menuName = "Enemy State Configs/Shoot State Config")]
public class ShootStateConfig : EnemyStateConfig
{
        [SerializeField] private List<ShootPatternInfo> possiblePatterns = new List<ShootPatternInfo>();
    
		public ShootPatternInfo GetRandomPatternInfo()
		{
			if (possiblePatterns.Count == 0)
			{
				Debug.LogError("No shoot patterns available.");
				return null;
			}

			int randomIndex = Random.Range(0, possiblePatterns.Count);
			return possiblePatterns[randomIndex];
		}
}
