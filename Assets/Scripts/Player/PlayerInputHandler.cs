using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    public Action<MouseButton> MouseClicked;
    public Action<InputDirection> LetterTyped;

    private bool isEnabled = false;

    public void EnableInput() => isEnabled = true;
    public void DisableInput() => isEnabled = false;

    public void UpdateMouseInput()
    {
        if (!isEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            MouseClicked?.Invoke(MouseButton.Left);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MouseClicked?.Invoke(MouseButton.Right);
        }
    }

    public void UpdateKeyboardInput()
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
        return InputDirection.None;
    }

    private bool AllowedInput(char _letter)
    {
        bool wasd = _letter == 'w' || _letter == 'a' || _letter == 's' || _letter == 'd';
        return wasd;
    }
}

public enum InputDirection
{
    None = -1,
    Up,
    Down,
    Left,
    Right
}

public enum MouseButton
{
    None = -1,
    Left,
    Right
}
