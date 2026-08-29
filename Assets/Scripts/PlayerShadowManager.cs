using UnityEngine;
using System.Collections.Generic;

public class PlayerShadowManager : MonoBehaviour
{
    public static PlayerShadowManager Instance { get; private set; }

    private List<PlayerShadow> _playerShadows = new List<PlayerShadow>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void RegisterPlayerShadow(PlayerShadow shadow)
    {
        if (!_playerShadows.Contains(shadow))
        {
            _playerShadows.Add(shadow);
        }
    }

    public void UnregisterPlayerShadow(PlayerShadow shadow)
    {
        if (_playerShadows.Contains(shadow))
        {
            _playerShadows.Remove(shadow);
        }
    }

    public void GameUpdate()
    {
        foreach (var shadow in _playerShadows)
        {
            if (shadow != null && shadow.gameObject.activeInHierarchy)
            {
                shadow.GameUpdate();
            }
        }
    }
}
