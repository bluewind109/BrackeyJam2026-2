using System.Collections.Generic;
using EnemyStates;
using UnityEngine;

[System.Serializable]
public class EnemyPhaseData
{
    [SerializeField] private List<EnemyState> _attackStates = new List<EnemyState>();
    public List<EnemyState> AttackStates => _attackStates;
}
