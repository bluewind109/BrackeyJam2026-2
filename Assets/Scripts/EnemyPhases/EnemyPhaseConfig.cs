using System.Collections.Generic;
using EnemyStates;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPhaseConfig", menuName = "Enemy Phase/Enemy Phase Config")]
public class EnemyPhaseConfig : ScriptableObject
{
    [SerializeField] private List<EnemyState> _attackStates = new List<EnemyState>();
}
