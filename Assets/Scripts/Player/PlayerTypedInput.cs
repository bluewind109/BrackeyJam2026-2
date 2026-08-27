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

        _typedInputText.text = string.Join(" ", inputDirections);
    }
}
