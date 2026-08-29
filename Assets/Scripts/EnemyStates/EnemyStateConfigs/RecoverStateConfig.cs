using UnityEngine;

[CreateAssetMenu(fileName = "RecoverStateConfig", menuName = "Enemy State Configs/Recover State Config")]
public class RecoverStateConfig : EnemyStateConfig
{
    [SerializeField] private float recoverDuration = 1f;

    public float RecoverDuration => recoverDuration;
}
