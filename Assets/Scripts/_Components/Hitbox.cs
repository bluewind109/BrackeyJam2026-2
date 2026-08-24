using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public System.Action<Hurtbox> onHit;

    private string targetTag = "";
    private BoxCollider2D hitboxCollider;

    void Awake()
    {
        hitboxCollider = GetComponent<BoxCollider2D>();
        if (hitboxCollider == null)
        {
            Debug.LogError("Hitbox requires a BoxCollider2D component.");
        }
    }

    public void Initialize(string targetTag)
    {
        this.targetTag = targetTag;
    }

    public void SetArea(Vector2 size)
    {
        transform.localScale = new Vector3(size.x, size.y, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Hitbox collided with: {other.transform.parent.name}, Tag: {other.transform.parent.tag}");
        if (other.transform.parent.CompareTag(targetTag))
        {
            var hurtbox = other.GetComponent<Hurtbox>();
            if (hurtbox != null)
            {
                onHit?.Invoke(hurtbox);
            }
        }
    }
}
