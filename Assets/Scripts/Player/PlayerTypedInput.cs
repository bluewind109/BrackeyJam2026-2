using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerTypedInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _typedInputText;

    public void UpdateTypedInput(List<InputDirection> inputDirections)
    {
        if (_typedInputText == null) return;
        if (inputDirections == null || inputDirections.Count == 0)
        {
            _typedInputText.text = string.Empty;
            return;
        }

        _typedInputText.text = string.Join(" ", inputDirections.ConvertAll(MapInputDirectionToArrowCharacter));
    }

    private string MapInputDirectionToArrowCharacter(InputDirection inputDirection)
    {
        switch (inputDirection)
        {
            case InputDirection.Up:
                return "↑";
            case InputDirection.Down:
                return "↓";
            case InputDirection.Left:
                return "←";
            case InputDirection.Right:
                return "→";
            default:
                Debug.LogError("Invalid input direction");
                return "↑"; // Default case
        }
    }
}
