using UnityEngine;
using MoreMountains.Tools;
public class Orb : MonoBehaviour
{
    [SerializeField] private MMFollowTarget _followTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._followTarget.Target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
