using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    public Action<InputDirection> LetterTyped;

    private bool isEnabled = false;

    void Start()
    {
        EnableInput(); // testing only
    }

    public void EnableInput() => isEnabled = true;
    public void DisableInput() => isEnabled = false;

    void Update() // testing only
    {
        UpdateInput();
    }

    public void UpdateInput()
    {
        if (!isEnabled) return;
        if (!Input.anyKeyDown) return;
        
        foreach (char letter in Input.inputString)
        {
            if (!AllowedInput(letter)) continue;
            InputDirection direction = CheckDirectionalInput(letter);
            Debug.Log($"Letter typed: {letter}, Direction: {direction}");
            LetterTyped?.Invoke(direction);
        }
    }

    private InputDirection CheckDirectionalInput(char letter)
    {
        switch (letter)
        {
            case 'w':
                return InputDirection.Up;
            case 's':
                return InputDirection.Down;
            case 'a':
                return InputDirection.Left;
            case 'd':
                return InputDirection.Right;
            default:
                Debug.LogError("Invalid input letter");
                break;
        }
        return InputDirection.Up; // Default return to satisfy compiler, though this should never be reached
    }

    private bool AllowedInput(char _letter)
    {
        bool wasd = _letter == 'w' || _letter == 'a' || _letter == 's' || _letter == 'd';
        return wasd;
    }
}

public enum InputDirection
{
    Up,
    Down,
    Left,
    Right
}
