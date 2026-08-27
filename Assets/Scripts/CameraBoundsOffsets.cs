using UnityEngine;

[DisallowMultipleComponent]
public class CameraBoundsOffsets : MonoBehaviour
{
    [SerializeField] private float _leftPixels;
    [SerializeField] private float _rightPixels;
    [SerializeField] private float _topPixels;
    [SerializeField] private float _bottomPixels;

    public float LeftPixels => Mathf.Max(0f, _leftPixels);
    public float RightPixels => Mathf.Max(0f, _rightPixels);
    public float TopPixels => Mathf.Max(0f, _topPixels);
    public float BottomPixels => Mathf.Max(0f, _bottomPixels);
}
