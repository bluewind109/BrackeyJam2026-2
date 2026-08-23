using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _inputHandler;

    void Start()
    {
        if (_inputHandler)
        {
            _inputHandler.EnableInput();
        }
    }

    void Update()
    {
        if (_inputHandler)
        {
            _inputHandler.UpdateInput();
        }
    }
}
