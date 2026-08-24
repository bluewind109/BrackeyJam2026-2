using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private Spell_FireBall _spell_FireBall;
    private int _inputIndex = 0;
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
        targetPosition.z = 0f;
        _currentSpell.Cast(transform.position, targetPosition);
        _currentSpell = null; // Reset the current spell after casting
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // Set this to the distance from the camera to the player
        // Debug.Log($"Mouse Position: {mousePosition}, World Position: {Camera.main.ScreenToWorldPoint(mousePosition)}");
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnLetterTyped(InputDirection input)
    {
        if (_spell_FireBall.IsInputMatched(input, _inputIndex))
        {
            _inputIndex++;
            _playerInputs.Add(input);
            Debug.Log($"Correct input: {input}. Current sequence: {string.Join(", ", _playerInputs)}");
            if (_inputIndex >= _spell_FireBall.InputSequence.Length)
            {
                Debug.Log("<color=green>Input sequence completed!</color>");
                _inputIndex = 0; // Reset for next sequence
                _currentSpell = _spell_FireBall; // Set the current spell to the completed spell
                _playerInputs.Clear();
            }
        }
        else
        {
            Debug.Log("Incorrect input. Resetting sequence.");
            _inputIndex = 0; // Reset on incorrect input
            _playerInputs.Clear();
        }
    }
}
