using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public System.Action<int> OnHit;

    public void SetTag(string tag)
    {
        this.gameObject.tag = tag;
    } 

    public void TakeDamage(int damage)
    {
        // Debug.Log($"Hurtbox took {damage} damage.");
        OnHit?.Invoke(damage);
    }
}
