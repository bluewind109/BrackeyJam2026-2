using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private Spell _spell;
    private int inputIndex = 0;
    private List<InputDirection> _playerInputs = new List<InputDirection>();

    void Start()
    {
        if (_inputHandler)
        {
            _inputHandler.EnableInput();
            _inputHandler.LetterTyped += OnLetterTyped;
        }
    }

    void OnDestroy()
    {
        if (_inputHandler)
        {
            _inputHandler.LetterTyped -= OnLetterTyped;
        }
    }

    void Update()
    {
        if (_inputHandler)
        {
            _inputHandler.UpdateInput();
        }
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
                Debug.Log("Input sequence completed!");
                inputIndex = 0; // Reset for next sequence
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
