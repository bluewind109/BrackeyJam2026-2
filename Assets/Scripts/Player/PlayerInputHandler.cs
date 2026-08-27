using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    public Action<MouseButton> MouseClicked;
    public Action<InputDirection> InputReceived;

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
        
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (!AllowedInput(keyCode)) continue;
                InputDirection direction = CheckDirectionalInput(keyCode);
                Debug.Log($"Key pressed: {keyCode}, Direction: {direction}");
                InputReceived?.Invoke(direction);
            }
        }
    }

    public Vector2 GetMovementInput()
    {
        if (!isEnabled) return Vector2.zero;

        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal -= 1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal += 1f;
        }

        float vertical = 0f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            vertical -= 1f;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            vertical += 1f;
        }

        Vector2 movementInput = new Vector2(horizontal, vertical);
        return movementInput.sqrMagnitude > 1f ? movementInput.normalized : movementInput;
    }

    private InputDirection CheckDirectionalInput(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.W:
            case KeyCode.UpArrow:
                return InputDirection.Up;
            case KeyCode.S:
            case KeyCode.DownArrow:
                return InputDirection.Down;
            case KeyCode.A:
            case KeyCode.LeftArrow:
                return InputDirection.Left;
            case KeyCode.D:
            case KeyCode.RightArrow:
                return InputDirection.Right;
            default:
                Debug.LogError($"Invalid input keyCode: {keyCode}");
                break;
        }
        return InputDirection.None;
    }

    private bool AllowedInput(KeyCode keyCode)
    {
        bool wasdKeys = keyCode == KeyCode.W || 
                        keyCode == KeyCode.A || 
                        keyCode == KeyCode.S || 
                        keyCode == KeyCode.D;
        bool arrowKeys = keyCode == KeyCode.UpArrow || 
                        keyCode == KeyCode.DownArrow || 
                        keyCode == KeyCode.LeftArrow || 
                        keyCode == KeyCode.RightArrow;
        return wasdKeys || arrowKeys;
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
