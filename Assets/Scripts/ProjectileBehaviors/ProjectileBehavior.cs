using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private GameObject _owner;
    private Vector3 _moveDirection;
    private float _speed;

    public void Initialize(GameObject owner, Vector3 moveDirection, float speed)
    {
        _owner = owner;
        _moveDirection = moveDirection;
        _speed = speed;
    }

    public void GameUpdate()
    {
        _owner.transform.position += _moveDirection * _speed * Time.deltaTime;
    }
}
