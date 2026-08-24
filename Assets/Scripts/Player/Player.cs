using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private Spell _spell;
    private int inputIndex = 0;
    private Spell _currentSpell = null;
    private List<InputDirection> _playerInputs = new List<InputDirection>();

    void Start()
    {
        if (_inputHandler)
        {
            _inputHandler.EnableInput();
            _inputHandler.MouseClicked += OnMouseClicked;
            _inputHandler.LetterTyped += OnLetterTyped;
        }
    }

    void OnDestroy()
    {
        if (_inputHandler)
        {
            _inputHandler.MouseClicked -= OnMouseClicked;
            _inputHandler.LetterTyped -= OnLetterTyped;
        }
    }

    void Update()
    {
        if (_inputHandler)
        {
            _inputHandler.UpdateMouseInput();
            _inputHandler.UpdateKeyboardInput();
        }
    }

    private void OnMouseClicked(MouseButton button)
    {
        if (button == MouseButton.None) return;
        if (_currentSpell == null) return;

        Vector3 targetPosition = GetMouseWorldPosition();
        _currentSpell.Cast(transform.position, targetPosition);
        _currentSpell = null; // Reset the current spell after casting
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // Set this to the distance from the camera to the player
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnLetterTyped(InputDirection input)
    {
        if (_spell.IsInputMatched(input, inputIndex))
        {
            inputIndex++;
            _playerInputs.Add(input);
            Debug.Log($"Correct input: {input}. Current sequence: {string.Join(", ", _playerInputs)}");
            if (inputIndex >= _spell.InputSequence.Length)
            {
                Debug.Log("<color=green>Input sequence completed!</color>");
                inputIndex = 0; // Reset for next sequence
                _currentSpell = _spell; // Set the current spell to the completed spell
                _playerInputs.Clear();
            }
        }
        else
        {
            Debug.Log("Incorrect input. Resetting sequence.");
            inputIndex = 0; // Reset on incorrect input
            _playerInputs.Clear();
        }
    }
}
