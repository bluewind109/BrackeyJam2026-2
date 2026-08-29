using System.Collections.Generic;
using UnityEngine;

public class InputSpriteManager : MonoBehaviour
{
    public static InputSpriteManager Instance { get; private set; }

    [SerializeField] private InputSpriteConfig _inputSpriteConfig;

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

    public Sprite GetInputSprite(InputDirection inputDirection)
    {
        if (_inputSpriteConfig == null)
        {
            Debug.LogError("InputSpriteConfig is not assigned in the InputSpriteManager.");
            return null;
        }

        return _inputSpriteConfig.GetInputSprite(inputDirection);
    }

    public Sprite GetInputSprite_FocusMode(InputDirection inputDirection)
    {
        if (_inputSpriteConfig == null)
        {
            Debug.LogError("InputSpriteConfig is not assigned in the InputSpriteManager.");
            return null;
        }

        return _inputSpriteConfig.GetInputSprite_FocusMode(inputDirection);
    }

    public Sprite GetInputSprite_FocusMode_Typed(InputDirection inputDirection)
    {
        if (_inputSpriteConfig == null)
        {
            Debug.LogError("InputSpriteConfig is not assigned in the InputSpriteManager.");
            return null;
        }

        return _inputSpriteConfig.GetInputSprite_FocusMode_Typed(inputDirection);
    }
}

