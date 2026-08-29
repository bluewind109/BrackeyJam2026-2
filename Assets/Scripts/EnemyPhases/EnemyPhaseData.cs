using System.Collections.Generic;
using EnemyStates;
using UnityEngine;
using System;

[Serializable]
public class EnemyPhaseData
{
    [SerializeField] private List<StateData> _stateData = new List<StateData>();

    public List<StateData> StateData => _stateData;

    public int GetTotalWeight()
    {
        int totalWeight = 0;
        foreach (var stateData in _stateData)
        {
            totalWeight += Mathf.Max(0, stateData.BaseWeight);
        }
        return totalWeight;
    }
}

[Serializable]
public class StateData
{
    [SerializeField] private EnemyState _state;
    [SerializeField] private int _baseWeight = 1;

    public EnemyState State => _state;
    public int BaseWeight => _baseWeight;
}
